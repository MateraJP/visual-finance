using System;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace VisualFinanceiro.Auth.Implementations
{

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        private readonly string[] claims;
        public AuthorizeAttribute() { }
        public AuthorizeAttribute(params string[] claims)
        {
            this.claims = claims;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = (IUser)context.HttpContext.Items["User"];
            if (user == null)
            {
                context.Result = new JsonResult(new { message = "Unauthorized" }) { StatusCode = StatusCodes.Status401Unauthorized };
            }
            else if (this.claims != null && (user.claims == null || !claims.Any(c => user.claims.Contains(c))))
            {
                context.Result = new JsonResult(new { message = "Forbidden" }) { StatusCode = StatusCodes.Status403Forbidden };
            }
        }
    }
}
