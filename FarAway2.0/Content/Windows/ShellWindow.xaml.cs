using FarAway2._0.Content.Controls;
using FarAway2._0.Content.Controls.UserControls;
using FarAway2._0.Entities.Enums;
using ModernWpf.Controls;
using System.Threading;

namespace FarAway2._0.Content.Windows
{
    /// <summary>
    /// Interaction logic for ShellWindow.xaml
    /// </summary>
    public partial class ShellWindow : Window
    {
        private bool _isReallyClose = true;
        private Users _user;
#if DEBUG
        public ShellWindow()
        {
            InitializeComponent();
            //WindowConfiguration(DbUtils.db.Users.Find(new Random().Next(1, DbUtils.db.Users.Count() + 1)));
            MainContentControl.Content = new Empty();
            LoadingContentControl.Content = new Loading();
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
            if (Item.Action == Tools.Collections.MainWindowActions.Account)
            {
                //Account content
                return;
            }
            if (Item.Action == Tools.Collections.MainWindowActions.Exit)
            {
                new AuthenticationWindow().Show();
                _isReallyClose = false;
                Close();
                return;
            }

            // Table display on main content of NavigationView
            // я ебал эту хуйню

            if (TableTypeAttribute.GetAttribute(Item.DatabaseTable) == TableTypes.ReferenceTables)
            {
                LoadingContentControl.Visibility = Visibility.Visible;
                MainContentControl.Visibility = Visibility.Hidden;

                    await Application.Current.Dispatcher.InvokeAsync(() =>
                    {
                        Thread.Sleep(3000);
                        MainContentControl.Content = new ReferenceTablesView(Item.DatabaseTable);
                        LoadingContentControl.Visibility = Visibility.Hidden;
                        MainContentControl.Visibility = Visibility.Visible;
                    });
            }


        }

        private void AutoSuggestBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {

        }
    }
}
