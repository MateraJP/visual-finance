namespace VisualFinanceiro.Auth
{
    /// <summary>
    /// IUser interface for IAuthServices and IUserRepository
    /// </summary>
    /// <remarks>Can be overrided</remarks>
    public interface IUser
    {
        /// <summary>
        /// Unique identifier database generated
        /// </summary>
        string Id { get; }
        /// <summary>
        /// Unique username identifier
        /// </summary>
        string Username { get; set; }
        /// <summary>
        /// Email for safety validation and password recover
        /// </summary>
        string Email { get; set; }
        /// <summary>
        /// Indicates if the email was verified for password reset 
        /// </summary>
        bool? IsEmailValid { get; set; }
        /// <summary>
        /// Collection of user claims
        /// </summary>
        string[] claims { get; }
        /// <summary>
        /// User profile picture
        /// </summary>
        string ProfilePic { get; }
    }
}
