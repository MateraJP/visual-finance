using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Threading.Tasks;
using VisualFinanceiro.Auth.Implementations;

namespace VisualFinanceiro.Auth.Controllers
{
    public class PreferenceController : Controller
    {
        private readonly IAuthServices services;
        private readonly IAfterUserInsert afterInsert;
        private readonly JsonSerializerOptions opts;
        public PreferenceController(IAuthServices services, IAfterUserInsert afterInsert)
        {
            this.services = services;
            this.afterInsert = afterInsert;
            opts = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
        }

        /// <summary>
        /// Save User Preferences
        /// </summary>
        [HttpPost, Route("api/preferences"), Authorize]
        public async Task<IActionResult> SavePreferences() //[FromBody] List<UserPreferences> preferences)
        {
            //var user = await services.InsertNewUser(this.Request.HttpContext.Connection.Id, request.Email, request.Pass);
            //if (user.authenticated)
            //    await afterInsert.AfterUserInsert(user.key);

            // TODO: Obter dados padrão do Usuario
            return new JsonResult(new { }, opts);

            // TODO: Fazer o override do método acima para buscar dados complementares do Usuário
        }

        /// <summary>
        /// Get User Preferences
        /// </summary>
        [HttpGet, Route("api/preferences/{key}"), Authorize]
        public async Task<IActionResult> GetPreferences([FromRoute] string key)
        {
            //var user = await services.InsertNewUser(this.Request.HttpContext.Connection.Id, request.Email, request.Pass);
            //if (user.authenticated)
            //    await afterInsert.AfterUserInsert(user.key);

            // TODO: Obter dados padrão do Usuario
            return new JsonResult(new { }, opts);

            // TODO: Fazer o override do método acima para buscar dados complementares do Usuário
        }

        /// <summary>
        /// Delete User Preferences
        /// </summary>
        [HttpDelete, Route("api/preferences/{key}"), Authorize]
        public async Task<IActionResult> DeletePreferences([FromRoute] string key)
        {
            //var user = await services.InsertNewUser(this.Request.HttpContext.Connection.Id, request.Email, request.Pass);
            //if (user.authenticated)
            //    await afterInsert.AfterUserInsert(user.key);

            // TODO: Obter dados padrão do Usuario
            return new JsonResult(new { }, opts);

            // TODO: Fazer o override do método acima para buscar dados complementares do Usuário
        }
    }
}
