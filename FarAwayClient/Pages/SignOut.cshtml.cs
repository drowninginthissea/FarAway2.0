using FarAwayClient.Tools;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FarAwayClient.Pages
{
    public class SignOutModel : PageModel
    {
        public IActionResult OnGet()
        {
            HttpContext.Session.Remove(Literals.UserSessionKey);
            return RedirectToPage("/SignIn");
        }
    }
}
