using System.Threading.Tasks;

namespace VisualFinanceiro.Auth
{
    public interface IAfterUserInsert
    {
        Task AfterUserInsert(string dataKey);
    }
}
