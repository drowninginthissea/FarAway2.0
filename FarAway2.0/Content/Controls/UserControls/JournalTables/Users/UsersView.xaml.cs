using FarAway2._0.BaseClasses;
using FarAway2._0.Content.Controls.UserControls.JournalTables.Branches;
using FarAway2._0.Content.Windows;
using Microsoft.EntityFrameworkCore;
using ModernWpf.Controls;
using System.Windows.Controls;

namespace FarAway2._0.Content.Controls.UserControls.JournalTables.Users
{
    public partial class UsersView : SearchableTableView
    {
        private bool _isOnlyClients;
        public UsersView(bool IsOnlyClients)
        {
            InitializeComponent();
            _isOnlyClients = IsOnlyClients;
        }
        public override async Task UpdateDataAsync()
        {
            UsersOutput.ItemsSource = SearchByUsers(await GetUsers());
        }
        private async Task<List<Entities.Users>> GetUsers()
        {
            if (_isOnlyClients)
                return await DbUtils.db.Users.Where(u => u.idRole == Entities.Enums.Roles.Client).ToListAsync();
            return await DbUtils.db.Users.ToListAsync();
        }
        private IEnumerable<Entities.Users> SearchByUsers(IEnumerable<Entities.Users> users)
        {
            return users.Where(u => u.Surname.Contains(TextToSearch) ||
                u.Name.Contains(TextToSearch) ||
                u.Patronymic.Contains(TextToSearch) ||
                $"{u.Surname} {u.Name} {u.Patronymic}".Contains(TextToSearch) ||
                u.Email.Contains(TextToSearch) ||
                u.Login.Contains(TextToSearch) ||
                new ReversePhoneNumberParser(u.PhoneNumber).ParsedPhoneNumber.Contains(TextToSearch) ||
                u.idRoleNavigation.RoleName.Contains(TextToSearch));
        }
        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            ContentDialog DialogOfEdit = new ContentDialog();
            UsersEdit Control = new UsersEdit(DialogOfEdit, UpdateDataAsync);
            StaticTools.OpenDialog(DialogOfEdit, Control);
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            ContentDialog DialogOfEdit = new ContentDialog();
            UsersEdit Control =
                new UsersEdit(
                    DialogOfEdit,
                    UpdateDataAsync,
                    (sender as Button).DataContext as Entities.Users);
            StaticTools.OpenDialog(DialogOfEdit, Control);
        }
    }
}
