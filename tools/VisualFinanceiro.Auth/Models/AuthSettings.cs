namespace VisualFinanceiro.Auth
{
    internal class AuthSettings : IAuthSettings
    {
        public int ExpirationHours { get; set; }
        public string Secret { get; set; }
    }
}
