using FarAwayClient.Models;
using FarAwayClient.Services;
using FarAwayClient.Tools;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace FarAwayClient.Pages
{
    public class SignInModel : PageModel
    {
        private Db context;
        private HashService hashService;
        public SignInModel(Db context, HashService hashing)
        {
            this.context = context;
            hashService = hashing;
        }
        [BindProperty]
        public InputModel Input { get; set; }
        public class InputModel
        {
            [Required (ErrorMessage = "Логин не указан")]
            public string Login { get; set; }
            [Required (ErrorMessage = "Пароль не введён")]
            public string Password { get; set; }
        }
        public void OnGet()
        {
        }
        public IActionResult OnPost()
        {
            if (!context.Users.Any(u => u.Login == Input.Login)) // user not found situation
            {
                ModelState.AddModelError("", "Неверный логин или пароль!");
                return Page();
            }
            User foundUser = context.Users.Single(u => u.Login == Input.Login);
            hashService.SetPassword(Input.Password);
            if (!hashService.VerifyWithThis(foundUser.Password)) // wrong password situation
            {
                ModelState.AddModelError("", "Неверный логин или пароль!");
                return Page();
            }
            HttpContext.Session.SetInt32(Literals.UserSessionKey, foundUser.Id);
            return RedirectToPage("/Account/Profile");
        }
    }
}
