using System.Threading.Tasks;

namespace VisualFinanceiro.Auth
{
    /// <summary>
    /// Check if tables used by IUserRepository exists, create if not exists. Override ISettupRepository and IUserRepository for custom table use.
    /// </summary>
    /// <remarks>Use IConnectionFactory</remarks>
    /// <remarks>Can be overrided</remarks>
    public interface ISettupRepository
    {
        /// <summary>
        /// Check if tables used by IUserRepository exists, create if not exists 
        /// </summary>
        Task CheckConfig();
    }
}
