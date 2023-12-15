using FarAway2._0.Entities;
using Microsoft.EntityFrameworkCore;
using ModernWpf.Controls;
using System.Text.RegularExpressions;
using System.Windows.Controls;

namespace FarAway2._0.Content.Controls.UserControls.JournalTables.Users
{
    public partial class UsersEdit : UserControl, IContentDialogParent
    {
        public ContentDialog ParentDialog { get; set; }
        private bool _isAddition = true;
        Entities.Users ChangingInstance;
        Func<Task> UpdateMethod;
        public UsersEdit(ContentDialog CallingDialog, Func<Task> UpdateMethod)
        {
            InitializeComponent();
            ParentDialog = CallingDialog;
            this.UpdateMethod = UpdateMethod;
            LoginTB.IsEnabled = true;
            FillComboBoxes();
        }
        public UsersEdit(ContentDialog CallingDialog, Func<Task> UpdateMethod, Entities.Users User)
        {
            InitializeComponent();
            ParentDialog = CallingDialog;
            this.UpdateMethod = UpdateMethod;
            FillComboBoxes();
            _isAddition = false;
            ChangingInstance = User;
            SurnameTB.Text = User.Surname;
            NameTB.Text = User.Name;
            PatronymicTB.Text = User.Patronymic;
            EmailTB.Text = User.Email;
            LoginTB.Text = User.Login;
            RolesCB.SelectedItem = User.idRoleNavigation;
            PhoneNumberTB.Text = new ReversePhoneNumberParser(User.PhoneNumber).ParsedPhoneNumber;
            AvatarSelector.OnConfigured += (sender, e) => {
                AvatarSelector.SetImage(ChangingInstance.Photo);
            };
        }
        private async void FillComboBoxes()
        {
            RolesCB.ItemsSource = await DbUtils.db.Roles.ToListAsync();
        }
        private void SaveChanges_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateAll())
                return;
            if (_isAddition)
            {
                if (DbUtils.db.Users.Any(u => u.Login == LoginTB.Text))
                {
                    MessageBox.Show("Пользователь с таким логином уже существует!");
                    return;
                }
                Entities.Users user = new Entities.Users()
                {
                    Surname = SurnameTB.Text,
                    Name = NameTB.Text,
                    Patronymic = PatronymicTB.Text,
                    Email = EmailTB.Text,
                    Login = LoginTB.Text,
                    PhoneNumber = new PhoneNumberParser(PhoneNumberTB.Text).ParsedPhoneNumber,
                    Password = new HashService(PasswordTB.Password).EncrypredPassword,
                    Photo = AvatarSelector.GetImage(),
                    idRole = (RolesCB.SelectedItem as Roles).id
                };
                DbUtils.db.Users.Add(user);
            }
            else
            {
                ChangingInstance.Surname = SurnameTB.Text;
                ChangingInstance.Name = NameTB.Text;
                ChangingInstance.Patronymic = PatronymicTB.Text;
                ChangingInstance.Email = EmailTB.Text;
                ChangingInstance.PhoneNumber = new PhoneNumberParser(PhoneNumberTB.Text).ParsedPhoneNumber;
                if (!(PasswordTB.Password == string.Empty))
                {
                    if (new HashService(PasswordTB.Password).VerifyWithThis(ChangingInstance.Password))
                    {
                        MessageBox.Show("Новый пароль для пользователя должен отличаться от текущего!", "Ошибка");
                        return;
                    }
                    if (string.IsNullOrWhiteSpace(PasswordTB.Password) || PasswordTB.Password.Length > 40)
                    {
                        MessageBox.Show("Новый пароль пользователя не введён корректно, либо превышен лимит символов (40)!", "Ошибка");
                        return;
                    }
                    ChangingInstance.Password = new HashService(PasswordTB.Password).EncrypredPassword;
                }
                ChangingInstance.Photo = AvatarSelector.GetImage();
                ChangingInstance.idRole = (RolesCB.SelectedItem as Roles).id;
            }
            DbUtils.db.SaveChanges();
            UpdateMethod();
            this.CloseContentDialog();
        }
        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            this.CloseContentDialog();
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
            if (_isAddition)
            {
                if (string.IsNullOrWhiteSpace(PasswordTB.Password) || PasswordTB.Password.Length > 40)
                {
                    MessageBox.Show("Пароль пользователя не введён, либо превышен лимит символов (40)!", "Ошибка");
                    return false;
                }
            }
            else
            {
                if (!(PasswordTB.Password == string.Empty))
                {
                    if (new HashService(PasswordTB.Password).VerifyWithThis(ChangingInstance.Password))
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
