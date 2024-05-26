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
        [Required(ErrorMessage = "������� �� �������")]
        [StringLength(40, MinimumLength = 3, ErrorMessage = "����� ������� ������ ���� �� 3 �� 40 ��������")]
        public string Surname { get; set; }

        [Required(ErrorMessage = "��� �� �������")]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "����� ����� ������ ���� �� 3 �� 30 ��������")]
        public string Name { get; set; }

        [StringLength(50, ErrorMessage = "����� �������� ������ ���� ����� 50 ��������")]
        public string? Patronymic { get; set; }

        [Required(ErrorMessage = "����������� ����� �� �������")]
        [EmailAddress(ErrorMessage = "������������ ������ ����������� �����")]
        public string Email { get; set; }

        [Required(ErrorMessage = "����� �� �����")]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "����� ������ ������ ���� �� 3 �� 30 ��������")]
        public string Login { get; set; }

        
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "������ �� ���������")]
        public string? ConfirmPassword { get; set; }

        [Required(ErrorMessage = "����� �������� �� �����")]
        [RegularExpression(@"\+7 \(\d{3}\) \d{3}-\d{2}-\d{2}", ErrorMessage = "������������ ������ ������ ��������")]
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
                    ModelState.AddModelError("", "����� � ������� ������ ���������! ���������� ������ ����� ������!");
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
