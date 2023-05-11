using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace VisualFinanceiro.WebApi.Controllers
{
    public abstract class BaseController : ControllerBase
    {
        protected IActionResult ResponderValidationException()
        {
            return this.BadRequest(ModelState);
        }

        protected IActionResult ResponderJsonResult<T>(T obj)
        {
            return new JsonResult(obj, GetJsonSerializerOptions());
        }
        
        protected JsonSerializerOptions GetJsonSerializerOptions()
        {
            var opt = new JsonSerializerOptions
            {
                AllowTrailingCommas = true,
                WriteIndented = true,
                PropertyNameCaseInsensitive = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                // ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };

            opt.Converters.Add(new JsonStringEnumConverter());
            return opt;
        }
    }
}
