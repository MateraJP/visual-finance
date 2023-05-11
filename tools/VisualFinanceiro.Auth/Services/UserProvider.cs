namespace VisualFinanceiro.Auth.Services
{
    internal class UserProvider : IUserProvider
    {
        private string tenant;
        private IUser user;
        public IUser GetUser()
        {
            return user;
        }

        public string GetTenant()
        {
            return tenant;
        }

        void IUserProvider.SetUser(IUser user)
        {
            this.user = user;
        }

        void IUserProvider.SetTenant(string tenant)
        {
            this.tenant = tenant;
        }
    }
}
