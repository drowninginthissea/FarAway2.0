using FarAwayClient.Models;
using FarAwayClient.Services;
using FarAwayClient.Tools;
using FarAwayClient.Tools.DbEnums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace FarAwayClient.Pages
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

        [Required(ErrorMessage = "Пароль не введён")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Подтверждение пароля не введено")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Номер телефона не введён")]
        [RegularExpression(@"\+7 \(\d{3}\) \d{3}-\d{2}-\d{2}", ErrorMessage = "Неправильный формат номера телефона")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Фото профиля не выбрано")]
        [Display(Name = "File")]
        public IFormFile Photo { get; set; }
    }
    public class SignUpModel : PageModel
    {
        private Db context;
        private HashService hashService;
        public SignUpModel(Db context, HashService hashService)
        {
            this.context = context;
            this.hashService = hashService;
        }
        [BindProperty]
        public InputModel Input { get; set; }
        public void OnGet() { }
        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (context.Users.Any(u => u.Login == Input.Login))
            {
				ModelState.AddModelError("", "Пользователь с таким логином уже существует!");
                return Page();
			}

            hashService.SetPassword(Input.Password);
            string hashedPassword = hashService.EncrypredPassword;

            string formattedPhoneNumber = Input.PhoneNumber.Replace("+", "").Replace(" ", "").Replace("(", "").Replace(")", "").Replace("-", "");

            byte[] photoBytes;
            using (MemoryStream memoryStream = new MemoryStream())
            {
                Input.Photo.CopyTo(memoryStream);
                photoBytes = memoryStream.ToArray();
            }

            User newUser = new User()
            {
                Surname = Input.Surname,
                Name = Input.Name,
                Patronymic = Input.Patronymic,
                Email = Input.Email,
                Login = Input.Login,
                Password = hashedPassword,
                PhoneNumber = formattedPhoneNumber,
                Photo = photoBytes,
                IdRole = Roles.Client
            };

            context.Add(newUser);
            context.SaveChanges();

            TempData["ShowModal"] = true;

            return RedirectToPage("/SignUp");
        }
    }
}
