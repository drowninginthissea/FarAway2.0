using FarAwayClient.Models;
using FarAwayClient.Tools;
using Microsoft.AspNetCore.Mvc;

namespace FarAwayClient.ViewComponents
{
    public class UserAuthentication : ViewComponent
    {
        private Db _context;
        public UserAuthentication(Db context)
        {
            _context = context;
        }
        public IViewComponentResult Invoke()
        {
            int? userId = HttpContext.Session.GetInt32(Literals.UserSessionKey);
            string? OutParametr = userId == null ? null : $"data:image/png;base64,{Convert.ToBase64String(_context.Users.Find(userId).Photo)}";
            return View<string>(OutParametr);
        }
    }
}
