using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Threading.Tasks;
using VisualFinanceiro.Auth.Implementations;

namespace VisualFinanceiro.Auth.Controllers
{
    public class ClaimController : Controller
    {
        private readonly IAuthServices services;
        private readonly IAfterUserInsert afterInsert;
        private readonly JsonSerializerOptions opts;
        public ClaimController(IAuthServices services, IAfterUserInsert afterInsert)
        {
            this.services = services;
            this.afterInsert = afterInsert;
            opts = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
        }
    }
}
