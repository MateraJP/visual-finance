using VisualFinanceiro.WebApi.Helpers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using System;
using System.Linq;
using System.Text.Json.Serialization;
using VisualFinanceiro.Auth;
using VisualFinanceiro.Negocios.Context;
using VisualFinanceiro.Negocios.Services;

namespace VisualFinanceiro.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Authentication(Configuration)
                .AuthUseSqlServer(Configuration.GetConnectionString("DefaultConnection"));

            services.AddDbContext<ControleContaContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<IAfterUserInsert, AfterUserInsertService>();
            services.AddScoped<IDatakeyProvider, DatakeyProvider>();

            services.AddControllers()
                .AddJsonOptions(opts => 
                {
                    opts.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                });

            // Exemplo de como sobrescrever a interface de Auth.
            //var providerDescriptor = new ServiceDescriptor(
            //    typeof(IUserRepository),
            //    typeof(RepoUsuarioUnimed),
            //    // Cuidado!!! Lifetime deve seguir a injeção original ou pode causar conflitos
            //    ServiceLifetime.Singleton); 
            //services.Replace(providerDescriptor);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors(configurePolicy =>
            {
                configurePolicy
                    .WithOrigins("http://localhost:4200")
                    .SetPreflightMaxAge(TimeSpan.FromSeconds(5))
                    .AllowCredentials()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .SetIsOriginAllowed(x => true);
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            var contentSecurity = Configuration.GetSection("AppSettings:ContentSecurity").Value;
            var contentSecurityKeys = Configuration.GetSection("AppSettings:ContentSecurityKey").Value.Split(',');

            app.Use(async (context, next) =>
            {
                context.Response.Headers.Append("Access-Control-Allow-Origin", "*");

                if (!context.Response.Headers.ContainsKey("Access-Control-Allow-Headers"))
                    context.Response.Headers.Add("Access-Control-Allow-Headers", new[] { "Content-Type" });

                if (!context.Response.Headers.ContainsKey("Access-Control-Allow-Methods"))
                    context.Response.Headers.Add("Access-Control-Allow-Methods", new[] { "*" });

                foreach (var key in contentSecurityKeys)
                {
                    if (!context.Response.Headers.ContainsKey(key))
                    {
                        context.Response.Headers.Add(key, new string[] { contentSecurity });
                    }
                }

                await next?.Invoke();
            });

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseCustomAuthentication();
            //app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
