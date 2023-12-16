using FarAway2._0.BaseClasses;
using FarAway2._0.Content.Controls.CustomControls;
using FarAway2._0.Content.Controls.UserControls;
using FarAway2._0.Content.Controls.UserControls.JournalTables.AdditionalServices;
using FarAway2._0.Content.Controls.UserControls.JournalTables.Branches;
using FarAway2._0.Content.Controls.UserControls.JournalTables.ParkingSpots;
using FarAway2._0.Content.Controls.UserControls.JournalTables.Rentals;
using FarAway2._0.Content.Controls.UserControls.JournalTables.Users;
using FarAway2._0.Content.Controls.UserControls.ReferenceTables;
using ModernWpf.Controls;
using System.Windows.Controls;

namespace FarAway2._0.Content.Windows
{
    public partial class ShellWindow : Window
    {
        private bool _isReallyClose = true;
        private ISearchable _currentMainContent;
        private Users _user;
#if DEBUG
        public ShellWindow()
        {
            InitializeComponent();
            WindowConfiguration(DbUtils.db.Users.Find(116));
        }
#endif
        public ShellWindow(Users user)
        {
            InitializeComponent();
            WindowConfiguration(user);
        }
        #region Window
        private void Window_Closed(object sender, EventArgs e)
        {
            if (_isReallyClose)
                Application.Current.Shutdown();
            else
                _isReallyClose = true;
        }
        private void WindowConfiguration(Users users)
        {
            _user = users;
            MainContentControl.Content = new Empty();
            LoadingContentControl.Content = new Loading();

            Title = "Главное окно Администратора";
            //Распределение ролей:
            if (users.idRole != Entities.Enums.Roles.Admin)
                ReferenceTables.Visibility = Visibility.Collapsed;
            switch (users.idRole) 
            {
                case Entities.Enums.Roles.Controller:
                    RentalsNavigation.Visibility = Visibility.Collapsed;
                    UserNavigation.Visibility = Visibility.Collapsed;
                    AddedServicesNavigation.Visibility = Visibility.Collapsed;
                    ReportingNavigation.Visibility = Visibility.Collapsed;
                    Title = "Главное окно Контролёра парковок";
                    break;
                case Entities.Enums.Roles.Manager:
                    BranchesNavigation.Visibility = Visibility.Collapsed;
                    SpotsNavigation.Visibility = Visibility.Collapsed;
                    Title = "Главное окно Менеджера по работе с клиентами";
                    break;
                case Entities.Enums.Roles.Director:
                    Title = "Главное окно Директора";
                    break;
            }
        }
        #endregion
        private async void MainNavigationView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            MyNavigationViewItem Item = args.SelectedItem as MyNavigationViewItem;
            if (Item.Action == MainWindowActions.Reporting)
            {
                SearchAutoSuggestBox.Text = string.Empty;
                SearchAutoSuggestBox.IsEnabled = false;

                MainContentControl.Content = new ReportingControl();
                _currentMainContent = null;
            }
            if (Item.Action == MainWindowActions.Account)
            {
                SearchAutoSuggestBox.Text = string.Empty;
                SearchAutoSuggestBox.IsEnabled = false;

                MainContentControl.Content = new AccountControl(_user);
                _currentMainContent = null;
            }
            if (Item.Action == MainWindowActions.Exit)
            {
                new AuthenticationWindow().Show();
                _isReallyClose = false;
                Close();
                return;
            }



            // Table display on main content of NavigationView
            if (TableTypeAttribute.GetAttribute(Item.DatabaseTable) == TableTypes.ReferenceTables)
            {
                await CallTableView(new ReferenceTablesView(Item.DatabaseTable));
            }
            // Non reference table callback to main content
            if (Item.DatabaseTable == TableNames.Branches)
            {
                await CallTableView(new BranchesView());
            }
            if (Item.DatabaseTable == TableNames.ParkingSpots)
            {
                await CallTableView(new ParkingSpotsView());
            }
            if (Item.DatabaseTable == TableNames.ParkingSpaceRental)
            {
                await CallTableView(new RentalsView());
            }
            if (Item.DatabaseTable == TableNames.Users)
            {
                await CallTableView(new UsersView(_user.idRole == Entities.Enums.Roles.Manager ? true : false));
            }
            if (Item.DatabaseTable == TableNames.AdditionalServicesForRent)
            {
                await CallTableView(new AdditionalServicesView());
            }
        }
        private async Task CallTableView(SearchableTableView instance)
        {
            MainNavigationView.IsEnabled = false;
            SwapVisibilitiesContentControls();

            await instance.UpdateDataAsync();

            MainContentControl.Content = instance;
            SearchAutoSuggestBox.IsEnabled = true;
            SearchAutoSuggestBox.Text = string.Empty;
            _currentMainContent = instance;
            MainNavigationView.IsEnabled = true;
            SwapVisibilitiesContentControls();
        }
        private void SwapVisibilitiesContentControls()
        {
            if (MainContentControl.Visibility == Visibility.Visible)
            {
                MainContentControl.Visibility = Visibility.Hidden;
                LoadingContentControl.Visibility = Visibility.Visible;
            }
            else
            {
                MainContentControl.Visibility = Visibility.Visible;
                LoadingContentControl.Visibility = Visibility.Hidden;
            }
        }

        private void SearchAutoSuggestBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            _currentMainContent.TextToSearch = SearchAutoSuggestBox.Text;
        }
    }
}
