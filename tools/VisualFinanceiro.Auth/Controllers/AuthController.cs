using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Threading.Tasks;

namespace VisualFinanceiro.Auth.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthServices services;
        private readonly IAfterUserInsert afterInsert;
        private readonly JsonSerializerOptions opts;
        public AuthController(IAuthServices services, IAfterUserInsert afterInsert)
        {
            this.services = services;
            this.afterInsert = afterInsert;
            opts = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
        }

        /// <summary>
        /// Create new User
        /// </summary>
        [HttpPost, Route("register")]
        public async Task<IActionResult> New([FromBody] AuthRequest request)
        {
            var user = await services.Register(Request.HttpContext.Connection.Id, request.Username, request.Email, request.Pass);
            if (user.authenticated)
                await afterInsert.AfterUserInsert(user.key);

            return new JsonResult(user, opts);
        }

        /// <summary>
        /// Get Authentication Token
        /// </summary>
        [HttpPost, Route("login")]
        public async Task<IActionResult> Login([FromBody] AuthRequest request)
        {
            return new JsonResult(await services.Login(Request.HttpContext.Connection.Id, request.Username, request.Pass), opts);
        }

        //[HttpPost, Route("recover-pass")]
        //public async Task<IActionResult> RecoverPass([FromBody] AuthRequest request)
        //{
        //    await Task.FromResult(0);

        //    return new JsonResult(new AuthResponse { error = $"-", error_description = $"Não implementado" }, opts);
        //}
    }
}
