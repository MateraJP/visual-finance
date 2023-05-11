using VisualFinanceiro.Auth;

namespace VisualFinanceiro.WebApi.Helpers
{
    public class DatakeyProvider: IDatakeyProvider
    {
        private readonly IUserProvider userProvider;
        public DatakeyProvider(IUserProvider userProvider)
        {
            this.userProvider = userProvider;
        }

        public string GetCurrentDataKey()
        {
            return (GetCurrentDataKey(userProvider.GetUser()));
        }

        public static string GetCurrentDataKey(IUser user = null)
        {
            // TODO: check for Tenant on request headers?
            return (user?.Id ?? "-1");
        }
    }
}
