﻿using FarAway2._0.BaseClasses;
using FarAway2._0.Content.Controls.CustomControls;
using FarAway2._0.Content.Controls.UserControls;
using FarAway2._0.Content.Controls.UserControls.JournalTables.Branches;
using FarAway2._0.Content.Controls.UserControls.JournalTables.ParkingSpots;
using FarAway2._0.Content.Controls.UserControls.JournalTables.Rentals;
using FarAway2._0.Content.Controls.UserControls.JournalTables.Users;
using FarAway2._0.Content.Controls.UserControls.ReferenceTables;
using FarAway2._0.Entities.Enums;
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
        }
        #endregion
        private async void MainNavigationView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            MyNavigationViewItem Item = args.SelectedItem as MyNavigationViewItem;
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
                await CallTableView(new UsersView());
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
