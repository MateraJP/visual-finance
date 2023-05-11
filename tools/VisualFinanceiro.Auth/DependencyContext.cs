using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace VisualFinanceiro.Auth
{
    public static class DependencyContext
    {
        /// <summary>
        /// Extensão para carregar as dependências
        /// </summary>
        /// <param name="services">todo: describe services parameter on Configure</param>
        /// <typeparam name="IConfiguration"> Represents a set of key/value application configuration properties. <see cref="IConfiguration"/></typeparam>
        public static IServiceCollection Authentication(this IServiceCollection services, IConfiguration configuration)
        {
            return services.Configure(configuration);
        }

        public static void UseCustomAuthentication(this IApplicationBuilder app)
        {
            app.UseMiddleware<JwtMiddleware>();
            app.ApplicationServices.GetService<ISettupRepository>();
        }
    }
}
