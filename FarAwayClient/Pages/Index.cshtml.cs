using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FarAwayClient.Pages
{
    public class IndexModel : PageModel
    {
        public IndexModel() { }
        public IActionResult OnGet()
        {
            return Page();
        }
    }
}
