using FarAway2._0.Content.Controls;
using FarAway2._0.Content.Controls.UserControls;
using FarAway2._0.Entities.Enums;
using ModernWpf.Controls;

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
            WindowConfiguration(DbUtils.db.Users.Find(new Random().Next(1, DbUtils.db.Users.Count() + 1)));
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
        }
        #endregion
        private void MainNavigationView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
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

            if (TableTypeAttribute.GetAttribute(Item.DatabaseTable) == TableTypes.ReferenceTables)
            {
                MainContentControl.Content = new ReferenceTablesView(Item.DatabaseTable);
            }

            
        }

        private void AutoSuggestBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {

        }
    }
}
