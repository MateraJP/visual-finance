using FluentValidation;

namespace VisualFinanceiro.Auth.Validation
{
    internal class AuthenticationSettingsValidator : AbstractValidator<AuthSettings>
    { 
        public AuthenticationSettingsValidator()
        {
            RuleFor(r => r.ExpirationHours).NotEmpty().WithMessage($"{nameof(AuthSettings.ExpirationHours)} cannot be null");
            RuleFor(r => r.Secret).NotEmpty().WithMessage($"{nameof(AuthSettings.Secret)} cannot be null");
        }
    }
}
