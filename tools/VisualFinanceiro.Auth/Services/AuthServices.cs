using System;
using System.Text;
using System.Threading.Tasks;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;

namespace VisualFinanceiro.Auth.Services
{
    internal class AuthServices : IAuthServices
    {
        #region [Constructor]

        private readonly string tokenSecret;
        private readonly int expirationHours;
        private readonly IUserRepository repository;
        private readonly Dictionary<string, IUserAuth> origins = new Dictionary<string, IUserAuth>();

        public AuthServices(IAuthSettings settings, IUserRepository repository)
        {
            tokenSecret = settings.Secret;
            expirationHours = settings.ExpirationHours;
            expirationHours = expirationHours < 1 ? 1 : expirationHours;
            this.repository = repository;
        }

        #endregion

        public async Task<AuthResponse> Register(string deviceId, string username, string email, string pass)
        {
            if (string.IsNullOrEmpty(deviceId))
                return new AuthResponse { error = $"Origin", error_description = $"requisição invalida.", authenticated = false };

            // Get origin
            origins.TryGetValue(deviceId, out var usuarioPrev);

            // Check temporary block
            if (usuarioPrev?.BlockTemp > DateTime.Now)
            {
                return new AuthResponse { error = $"Login", error_description = $"bloquei temporário, aguarde [{usuarioPrev?.BlockTemp - DateTime.Now:mm:ss}].", authenticated = false };
            }

            // Clear blocks after 10 minutes
            if (usuarioPrev?.BlockTemp?.AddMinutes(10) < DateTime.Now)
            {
                usuarioPrev = null;
            }

            try
            {
                if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(pass))
                    throw new ArgumentException("requisição invalida.", string.IsNullOrEmpty(username) ? "username" : string.IsNullOrEmpty(email) ? "email" : "password");

                // Try to Insert new user
                usuarioPrev = await repository.InsertUser(username, email, pass, false) as IUserAuth;
                usuarioPrev.IsAuth = true;

                // TODO: Create main Tenant
                // TODO: Create Claims for main Tenant
            }
            catch (ArgumentException ex)
            {
                // Username alread been used

                // Previous tries or new try from Origin
                usuarioPrev = usuarioPrev ?? new User { };
                usuarioPrev.IsAuth = false;
                usuarioPrev.Tries++;

                // Add to Origins list
                origins.TryAdd(deviceId, usuarioPrev);

                if (usuarioPrev.Tries < 5)
                {
                    // If not exceeded 5 tries show error message
                    usuarioPrev.BlockTemp = DateTime.Now;
                    if (string.IsNullOrEmpty(usuarioPrev.InvalidMessage))
                        usuarioPrev.InvalidMessage = $"{ex.Message}.";
                }
                else
                {
                    // If exceeded 5 tries lock user for 1.5 minutes
                    usuarioPrev.BlockTemp = DateTime.Now.AddSeconds(90);
                    usuarioPrev.InvalidMessage = $"bloqueado por número de tentativas, aguarde {usuarioPrev?.BlockTemp - DateTime.Now:mm:ss} e tente novamente";
                }
            }

            if (!(usuarioPrev?.IsAuth ?? false))
                return new AuthResponse { error = $"Login", error_description = usuarioPrev?.InvalidMessage, authenticated = false };
            else
                return GenerateJwtToken(usuarioPrev);
        }

        public async Task<AuthResponse> Login(string deviceId, string username, string pass)
        {
            if (string.IsNullOrEmpty(deviceId))
            {
                return new AuthResponse { error = $"Origin", error_description = $"requisição invalida.", authenticated = false };
            }

            // Get origin
            origins.TryGetValue(deviceId, out var usuarioPrev);

            // Check temporary block
            if (usuarioPrev?.BlockTemp > DateTime.Now)
            {
                return new AuthResponse { error = $"Login", error_description = $"bloquei temporário, aguarde [{usuarioPrev?.BlockTemp - DateTime.Now:mm:ss}].", authenticated = false };
            }

            // Clear blocks after 10 minutes
            if (usuarioPrev?.BlockTemp?.AddMinutes(10) < DateTime.Now)
            {
                usuarioPrev = null;
            }

            try
            {
                if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(pass))
                {
                    throw new ArgumentException("requisição invalida.");
                }

                if (await repository.IsAuth(username, pass))
                {
                    var usuario = await repository.GetUserByUsername(username);
                    usuarioPrev = usuario as IUserAuth;
                    usuarioPrev.IsAuth = true;
                }
                else
                {
                    throw new Exception("login ou senha inválidos.");
                }
            }
            catch (Exception ex)
            {
                // Username and Password didn't match

                // Previous tries or new try from Origin
                usuarioPrev = usuarioPrev ?? new User { };
                usuarioPrev.IsAuth = false;
                usuarioPrev.Tries++;

                // Add to Origins list
                origins.TryAdd(deviceId, usuarioPrev);

                if (usuarioPrev.Tries < 5)
                {
                    // If not exceeded 5 tries show error message
                    usuarioPrev.BlockTemp = DateTime.Now;
                    if (string.IsNullOrEmpty(usuarioPrev.InvalidMessage))
                        usuarioPrev.InvalidMessage = $"{ex.Message}.";
                }
                else
                {
                    // If exceeded 5 tries lock user for 1.5 minutes
                    usuarioPrev.BlockTemp = DateTime.Now.AddSeconds(90);
                    usuarioPrev.InvalidMessage = $"bloqueado por número de tentativas, aguarde {usuarioPrev?.BlockTemp - DateTime.Now:mm:ss} e tente novamente";
                }
            }


            if (!(usuarioPrev?.IsAuth ?? false))
                return new AuthResponse { error = $"Login", error_description = usuarioPrev?.InvalidMessage, authenticated = false };
            else
                return GenerateJwtToken(usuarioPrev);
        }

        #region [Private Methods]

        private AuthResponse GenerateJwtToken(IUser user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(tokenSecret);

            var created = DateTime.Now;
            var claims = new List<Claim>(new[]
            {
                new Claim("Id", user.Id.ToString()),
                new Claim("Email", user.Email),
                new Claim("Username", user.Username)
            });

            foreach (var claim in user.claims)
                claims.Add(new Claim(claim, claim));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),

                Expires = created.AddHours(expirationHours),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);

            var auth = new AuthResponse
            {
                key = user.Id,
                accessToken = tokenHandler.WriteToken(token),
                authenticated = true,
                created = created,
                expiration = tokenDescriptor.Expires.Value
            };
            return auth;
        }

        #endregion
    }
}
