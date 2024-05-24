using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace FarAwayClient.Tools
{
    public class CustomAuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var session = context.HttpContext.Session;
            if (session == null || session.GetInt32(Literals.UserSessionKey) == null)
            {
                context.Result = new RedirectToPageResult("/SignIn");
            }
        }
    }
}
