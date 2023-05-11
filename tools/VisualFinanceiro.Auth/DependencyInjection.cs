using Azure.Storage.Blobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VisualFinanceiro.Auth.Implementations;
using VisualFinanceiro.Auth.Interfaces;
using VisualFinanceiro.Auth.Services;

namespace VisualFinanceiro.Auth
{
    public static class DependencyInjection
    {
        internal static IServiceCollection Configure(this IServiceCollection services, IConfiguration configuration)
        {
            configuration.ValidateAuthentication();

            services.AddSingleton<IAuthSettings>(configuration.GetSection(nameof(AuthSettings)).Get<AuthSettings>());

            services.AddSingleton<ISettupRepository, SettupRepository>();
            services.AddSingleton<IUserRepository, UserRepository>();
            services.AddSingleton<IAuthServices, AuthServices>();

            services.AddScoped<IAfterUserInsert, AfterUserInsert>();
            services.AddScoped<IUserProvider, UserProvider>();

            services.AddScoped(x => new BlobServiceClient(configuration.GetConnectionString("AzureBlobStorage")));
            services.AddScoped<IBlobService, BlobService>();
            return services;
        }

        public static void AuthUseSqlServer(this IServiceCollection services, string connectionString)
        {
            services.AddSingleton<IConnectionFactory>(new ConnectionFactory(connectionString));
        }
    }
}
