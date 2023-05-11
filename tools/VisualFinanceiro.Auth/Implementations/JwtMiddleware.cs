using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualFinanceiro.Auth
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate next;
        private readonly IAuthSettings settings;

        public JwtMiddleware(RequestDelegate next, IAuthSettings settings)
        {
            this.next = next;
            this.settings = settings;
        }

        public async Task InvokeAsync(HttpContext context, IUserRepository repository)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (token != null)
                await attachUserToContext(context, repository, token);

            await next(context);
        }

        private async Task attachUserToContext(HttpContext context, IUserRepository repository, string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(settings.Secret);
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                
                // attach token user to context on successful jwt validation
                // for better performance
                //var id = Convert.ToString(jwtToken.Claims.First(x => x.Type == "Id").Value);
                //var email = Convert.ToString(jwtToken.Claims.First(x => x.Type == "Email").Value);
                //var claims = jwtToken.Claims.Where(c => c.Type == c.Value).Select(c => c.Value);
                //var t = jwtToken.Claims as IUser;
                //var user = new User(id) { Email = email, claims = claims.ToArray() };
                //context.Items["User"] = user;
                //IUserProvider userProvider = (context.RequestServices.GetService(typeof(IUserProvider)) as IUserProvider);
                //userProvider?.SetUser(user);

                // attach database user to context on successful jwt validation
                // for better uptodate access
                var userId = long.Parse(jwtToken.Claims.First(x => x.Type == "Id").Value);
                var user = await repository.GetUserById(userId);
                IUserProvider userProvider = (context.RequestServices.GetService(typeof(IUserProvider)) as IUserProvider);
                userProvider?.SetUser(user);

                var tenant = context.Request.Headers["Tenant"].FirstOrDefault() ?? userId.ToString();
                userProvider?.SetTenant(tenant);
                context.Items["User"] = user;
            }
            catch
            {
                // do nothing if jwt validation fails
                // user is not attached to context so request won't have access to secure routes
            }
        }
    }
}
