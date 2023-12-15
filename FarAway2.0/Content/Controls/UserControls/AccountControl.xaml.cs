using FarAway2._0.Entities;
using Microsoft.Office.Interop.Word;
using System.Text.RegularExpressions;
using System.Windows.Controls;

namespace FarAway2._0.Content.Controls.UserControls
{
    public partial class AccountControl : UserControl
    {
        public Users User { get; set; }
        public AccountControl(Users user)
        {
            InitializeComponent();
            InitializeControl(user);
        }
        private void InitializeControl(Users user)
        {
            User = user;
            RoleNameInfo.Text = user.idRoleNavigation.RoleName;
            RoleFuncsInfo.Text = user.idRole switch
            {
                Entities.Enums.Roles.Admin => Properties.Resources.AdminFuncs,
                Entities.Enums.Roles.Controller => Properties.Resources.ControllerFuncs,
                Entities.Enums.Roles.Manager => Properties.Resources.ManagerFuncs,
                Entities.Enums.Roles.Director => Properties.Resources.DirectorFuncs
            };
            SetFields();
            AvatarSelector.OnConfigured += (sender, e) => {
                AvatarSelector.SetImage(User.Photo);
            };
        }
        private void SetFields()
        {
            SurnameTB.Text = User.Surname;
            NameTB.Text = User.Name;
            PatronymicTB.Text = User.Patronymic;
            EmailTB.Text = User.Email;
            LoginTB.Text = User.Login;
            PhoneNumberTB.Text = new ReversePhoneNumberParser(User.PhoneNumber).ParsedPhoneNumber;
        }
        private double GetSpacing()
        {
            double HeightOfTBs = SurnameTB.ActualHeight * 4;
            double EmptySpace = LeftSideContainer.ActualHeight - HeightOfTBs;
            return EmptySpace / 3;
        }
        private void TextBoxesGrid_OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            LeftStackPanel.Spacing = GetSpacing();
            RightStackPanel.Spacing = GetSpacing();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateAll())
                return;

            User.Name = NameTB.Text;
            User.Surname = SurnameTB.Text;
            User.Patronymic = PatronymicTB.Text;
            User.Email = EmailTB.Text;
            User.PhoneNumber = new PhoneNumberParser(PhoneNumberTB.Text).ParsedPhoneNumber;
            if (!(PasswordTB.Password == string.Empty))
            {
                if (new HashService(PasswordTB.Password).VerifyWithThis(User.Password))
                {
                    MessageBox.Show("Новый пароль для пользователя должен отличаться от текущего!", "Ошибка");
                    return;
                }
                if (string.IsNullOrWhiteSpace(PasswordTB.Password) || PasswordTB.Password.Length > 40)
                {
                    MessageBox.Show("Новый пароль пользователя не введён корректно, либо превышен лимит символов (40)!", "Ошибка");
                    return;
                }
                User.Password = new HashService(PasswordTB.Password).EncrypredPassword;
            }
            User.Photo = AvatarSelector.GetImage();
            DbUtils.db.SaveChanges();
            MessageBox.Show("Данные успешно обновлены!", "Успешно");
            SetFields();
            PasswordTB.Password = "";
            RepeatPasswordTB.Password = "";
        }
        #region Validation
        private bool ValidateAll()
        {
            if (string.IsNullOrWhiteSpace(SurnameTB.Text) || SurnameTB.Text.Length > 40)
            {
                MessageBox.Show("Фамилия пользователя не введена, либо лимит символов превышен (40)!", "Ошибка");
                return false;
            }
            if (string.IsNullOrWhiteSpace(NameTB.Text) || NameTB.Text.Length > 30)
            {
                MessageBox.Show("Имя пользователя не введено, либо лимит символов превышен (30)!", "Ошибка");
                return false;
            }
            if (PatronymicTB.Text.Length > 50)
            {
                MessageBox.Show("Лимит символов на ввод отчества (50) превышен!", "Ошибка");
                return false;
            }
            if (!ValidateEmail(EmailTB.Text))
                return false;
            if (!PhoneNumberTB.IsMaskCompleted)
            {
                MessageBox.Show("Номер телефона введён не полностью!", "Ошибка");
                return false;
            }
            if (PasswordTB.Password != RepeatPasswordTB.Password)
            {
                MessageBox.Show("Введённые пароли не совпадают друг с другом!", "Ошибка");
                return false;
            }
            if (!(PasswordTB.Password == string.Empty))
            {
                if (new HashService(PasswordTB.Password).VerifyWithThis(User.Password))
                {
                    MessageBox.Show("Новый пароль для пользователя должен отличаться от текущего!", "Ошибка");
                    return false;
                }
                if (string.IsNullOrWhiteSpace(PasswordTB.Password) || PasswordTB.Password.Length > 40)
                {
                    MessageBox.Show("Новый пароль пользователя не введён корректно, либо превышен лимит символов (40)!", "Ошибка");
                    return false;
                }
            }
            if (!AvatarSelector.IsImageSet())
            {
                MessageBox.Show("Необходимо выбрать изображение для аватара пользователя!", "Ошибка");
                return false;
            }
            return true;
        }
        private bool ValidateEmail(string email)
        {
            string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
            if (string.IsNullOrWhiteSpace(email))
            {
                MessageBox.Show("Электронная почта не введена, либо введены пустые символы!", "Ошибка");
                return false;
            }
            if (!Regex.IsMatch(email, pattern))
            {
                MessageBox.Show("Электронная почта введена неверно! Проверьте правильность ввода!", "Ошибка");
                return false;
            }
            return true;
        }
        #endregion
    }
}
