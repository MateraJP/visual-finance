using System;
using System.IO;
using System.Threading.Tasks;

namespace VisualFinanceiro.Auth
{
    /// <summary>
    /// IUserRepository used by AuthServices to access database
    /// </summary>
    /// <remarks>Use IConnectionFactory</remarks>
    /// <remarks>Can be overrided</remarks>
    public interface IUserRepository
    {
        /// <summary>
        /// Provides the user entity from database 
        /// </summary>
        /// <remarks>Throws ArgumentException id not exists</remarks>
        /// <param name="id">Unique identifier</param>
        /// <returns>IUser</returns>
        Task<IUser> GetUserById(long id);

        /// <summary>
        /// Provides the user entity from database 
        /// </summary>
        /// <remarks>Throws ArgumentException email not exists</remarks>
        /// <param name="username">Unique username identifier</param>
        /// <returns>IUser</returns>
        Task<IUser> GetUserByUsername(string username);

        /// <summary>
        /// Create new user
        /// </summary>
        /// <remarks>Throws ArgumentException email already in table</remarks>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        Task<IUser> InsertUser(string username, string email, string password, bool isEmailValid);

        /// <summary>
        /// Updates flag after validation
        /// </summary>
        /// <param name="username">Unique email identifier</param>
        /// <param name="file">Unique username identifier</param>
        Task<IUser> SetProfilePic(string username, string path);

        /// <summary>
        /// Updates flag after validation
        /// </summary>
        /// <param name="email">Unique email identifier</param>
        /// <param name="isEmailValid">Indicates if the email was verified for password reset </param>
        Task<IUser> SetEmailValid(long id, bool isEmailValid);

        /// <summary>
        /// Check if the providade email and password are valid
        /// </summary>
        /// <param name="email">Unique email identifier</param>
        /// <param name="password">User personal password</param>
        /// <returns>Boolean indicating if its a match</returns>
        Task<bool> IsAuth(string email, string password);
    }
}
