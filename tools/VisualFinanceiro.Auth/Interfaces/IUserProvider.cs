namespace VisualFinanceiro.Auth
{
    public interface IUserProvider
    {
        IUser GetUser();
        string GetTenant();
        internal void SetUser(IUser user);
        internal void SetTenant(string tenant);
    }
}
