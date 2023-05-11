using System.Data;
using System.Threading.Tasks;

namespace VisualFinanceiro.Auth
{
    /// <summary>
    /// IConnectionFactory provides a single open IDbConnection for IUserRepository and ISettupRepository
    /// </summary>
    /// <remarks>Can be overrided</remarks>
    public interface IConnectionFactory
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Task<IDbConnection> getConnection();
    }
}
