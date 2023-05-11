using FluentValidation;
using Microsoft.Extensions.Configuration;
using System;
using VisualFinanceiro.Auth.Validation;

namespace VisualFinanceiro.Auth
{
    public static class ValidateSettings
    {
        public static void ValidateAuthentication(this IConfiguration configuration)
        {
            var authSettings = configuration.GetSection(nameof(AuthSettings)).Get<AuthSettings>()
                ?? throw new NullReferenceException($"A Section '{nameof(AuthSettings)}' não foi encontrada no appsettings.json");

            var validatorAuth = new AuthenticationSettingsValidator();
            validatorAuth.ValidateAndThrow(authSettings);
        }
    }
}
