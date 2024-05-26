using FarAwayClient.Models;
using FarAwayClient.Services;
using FarAwayClient.Tools;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace FarAwayClient.Pages.Account
{
    public class InputModel
    {
        [Required(ErrorMessage = "Фамилия не введена")]
        [StringLength(40, MinimumLength = 3, ErrorMessage = "Длина фамилии должна быть от 3 до 40 символов")]
        public string Surname { get; set; }

        [Required(ErrorMessage = "Имя не введено")]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "Длина имени должна быть от 3 до 30 символов")]
        public string Name { get; set; }

        [StringLength(50, ErrorMessage = "Длина отчества должна быть менее 50 символов")]
        public string? Patronymic { get; set; }

        [Required(ErrorMessage = "Электронная почта не введена")]
        [EmailAddress(ErrorMessage = "Неправильный формат электронной почты")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Логин не введён")]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "Длина логина должна быть от 3 до 30 символов")]
        public string Login { get; set; }

        
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        public string? ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Номер телефона не введён")]
        [RegularExpression(@"\+7 \(\d{3}\) \d{3}-\d{2}-\d{2}", ErrorMessage = "Неправильный формат номера телефона")]
        public string PhoneNumber { get; set; }

        [Display(Name = "File")]
        public IFormFile? Photo { get; set; }
    }

    [CustomAuthorize]
    public class ProfileModel : PageModel
    {
        public Db context;
        public HashService hash;
        
        public ProfileModel(Db context, HashService hash)
        {
            this.hash = hash;
            this.context = context;
        }
        [BindProperty]
        public InputModel Input { get; set; }
        public IActionResult OnGetOut()
        {
            HttpContext.Session.Remove(Literals.UserSessionKey);
            return RedirectToPage("/SignIn");
        }
        public void OnGet() { }
        public IActionResult OnPost()
        {
            Models.User user = context.Users.Find(HttpContext.Session.GetInt32(Literals.UserSessionKey));
            if (Input.Password != null)
            {
                hash.SetPassword(Input.Password);
                if (hash.VerifyWithThis(user.Password))
                {
                    ModelState.AddModelError("", "Новый и текущий пароли совпадают! Необходимо ввести новый пароль!");
                    TempData["ScrollToForm"] = true;
                    return Page();
                }

                user.Password = hash.EncrypredPassword;
            }

            user.Surname = Input.Surname;
            user.Name = Input.Name;
            user.Patronymic = Input.Patronymic;
            user.Email = Input.Email;
            if (Input.Photo != null)
            {
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    Input.Photo.CopyTo(memoryStream);
                    user.Photo = memoryStream.ToArray();
                }
            }
            context.SaveChanges();

            TempData["ShowModal"] = true;
            return RedirectToPage("/Account/Profile");
        }
    }
}
