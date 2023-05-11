namespace VisualFinanceiro.Auth
{
    public interface IAuthSettings
    {
        int ExpirationHours { get; set; }
        string Secret { get; }
    }
}
