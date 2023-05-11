using System.Threading.Tasks;

namespace VisualFinanceiro.Auth.Services
{
    internal class AfterUserInsert : IAfterUserInsert
    {
        async Task IAfterUserInsert.AfterUserInsert(string dataKey)
        {
            await Task.CompletedTask;
        }
    }
}
