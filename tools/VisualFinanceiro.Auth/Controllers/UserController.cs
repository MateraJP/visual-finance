using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using VisualFinanceiro.Auth.Implementations;
using VisualFinanceiro.Auth.Interfaces;
using static System.Net.WebRequestMethods;

namespace VisualFinanceiro.Auth.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserRepository userRepository;
        private readonly IUserProvider userProvider;
        private readonly IBlobService blobService;
        private readonly JsonSerializerOptions opts;
        public UserController(IUserRepository userRepository,
            IUserProvider userProvider,
            IBlobService blobService)
        {
            this.userRepository = userRepository;
            this.userProvider = userProvider;
            this.blobService = blobService;
            opts = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
        }

        /// <summary>
        /// Get User Info
        /// </summary>
        [HttpGet, Route("api/user"), Authorize]
        public async Task<IActionResult> GetUser()
        {
            // TODO: Obter dados atravez de uma service que pode ter override pelo modulo que execute o auth
            var user = userProvider.GetUser();
            return new JsonResult(user, opts);
        }

        /// <summary>
        /// Get User Info
        /// </summary>
        [HttpPost, Route("api/user/profile-pic"), Authorize]
        public async Task<IActionResult> SaveProfilePic(string handle)
        {
            var files = Request.Form.Files[0];
            
            using (Stream file = files.OpenReadStream()) //Request.BodyReader.AsStream())
            {
                var user = userProvider.GetUser();
                if (!string.IsNullOrEmpty(user.ProfilePic))
                {
                    await blobService.DeleteAsync("profile-pic", user.ProfilePic.Split('/').LastOrDefault());
                }

                var filename = $"{Guid.NewGuid()}_{user.Id}";
                var uri = await blobService.SaveAsync(file, "profile-pic", filename, true);
                
                user = await userRepository.SetProfilePic(user.Username, uri);
                return new JsonResult(user, opts);
            }
        }

        //[HttpPost, Route("change-pass"), Authorize]
        //public async Task<IActionResult> ChangePass([FromBody] AuthRequest request)
        //{
        //    await Task.FromResult(0);

        //    return new JsonResult(new AuthResponse { error = $"-", error_description = $"Não implementado" }, opts);
        //}
    }
}
