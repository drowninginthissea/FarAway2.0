using FarAwayClient.Tools;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FarAwayClient.Pages.Account
{
    [CustomAuthorize]
    public class SuccessCheckedOutModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
