using Azure.Core;
using FarAway2._0.Interfaces;
using FarAway2._0.Tools.Extensions;
using ModernWpf.Controls;
using System.Text.RegularExpressions;
using System.Windows.Controls;
using static System.Net.Mime.MediaTypeNames;

namespace FarAway2._0.Content.Controls.UserControls.DataEdit
{
    public partial class ServiceProvidersEdit : UserControl, IContentDialogParent
    {
        public ContentDialog ParentDialog { get; set; }
        private bool _isAddition = true;
        ServiceProviders ChangingInstance;
        Func<Task> UpdateMethod;
        public ServiceProvidersEdit(ContentDialog CallingDialog, Func<Task> UpdateMethod)
        {
            InitializeComponent();
            ParentDialog = CallingDialog;
            this.UpdateMethod = UpdateMethod;
        }
        public ServiceProvidersEdit(ContentDialog CallingDialog, ServiceProviders Instance, Func<Task> UpdateMethod)
        {
            InitializeComponent();
            ParentDialog = CallingDialog;
            this.UpdateMethod = UpdateMethod;
            _isAddition = false;
            ChangingInstance = Instance;
            Name.Text = Instance.Name;
            ITIN.Text = Instance.ITIN;
            Address.Text = Instance.Address;
            PhoneNumber.Text = new ReversePhoneNumberParser(Instance.PhoneNumber).ParsedPhoneNumber;
            Email.Text = Instance.Email;
        }

        private void SaveChanges_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Name.Text))
            {
                MessageBox.Show("Значение названия поставщика не может быть пустым!", "Ошибка сохранения");
                return;
            }
            if (!Regex.IsMatch(ITIN.Text, @"^\d{10}$"))
            {
                MessageBox.Show("Значение ИНН должно сожержать 10 цифр!", "Ошибка сохранения");
                return;
            }
            if (string.IsNullOrWhiteSpace(Address.Text))
            {
                MessageBox.Show("Значение адреса поставщика не может быть пустым!", "Ошибка сохранения");
                return;
            }
            if (!PhoneNumber.IsMaskCompleted)
            {
                MessageBox.Show("Значение номера телефона поставщика должно быть заполнено полностью!", "Ошибка сохранения");
                return;
            }
            if (string.IsNullOrWhiteSpace(Email.Text))
            {
                MessageBox.Show("Значение электронной почты поставщика должно быть заполнено полностью!", "Ошибка сохранения");
                return;
            }
            if (!Regex.IsMatch(Email.Text, @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$"))
            {
                MessageBox.Show("Значение электронной почты заполнено не верно!", "Ошибка сохранения");
                return;
            }
            if (_isAddition)
            {
                ServiceProviders Instance = new ServiceProviders()
                {
                    Name = Name.Text,
                    ITIN = ITIN.Text,
                    Address = Address.Text,
                    PhoneNumber = new PhoneNumberParser(PhoneNumber.Text).ParsedPhoneNumber,
                    Email = Email.Text,
                };
                DbUtils.db.ServiceProviders.Add(Instance);
            }
            else
            {
                ChangingInstance.Name = Name.Text;
                ChangingInstance.ITIN = ITIN.Text;
                ChangingInstance.Address = Address.Text;
                ChangingInstance.PhoneNumber = new PhoneNumberParser(PhoneNumber.Text).ParsedPhoneNumber;
                ChangingInstance.Email = Email.Text;
            }
            DbUtils.db.SaveChanges();
            UpdateMethod();
            this.CloseContentDialog();
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            this.CloseContentDialog();
        }
    }
}
