using System.Threading.Tasks;

namespace VisualFinanceiro.Auth
{

    public interface IAuthServices
    {
        Task<AuthResponse> Register(string deviceId, string username, string email, string pass);
        Task<AuthResponse> Login(string deviceId, string username, string pass);
    }
}
