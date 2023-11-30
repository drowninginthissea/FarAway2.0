using System;
using System.Linq;
using System.Windows;
using FarAway2._0.Entities;
using FarAway2._0.Tools;
using ModernWpf.Controls;

namespace FarAway2._0.Content.Windows
{
    /// <summary>
    /// Interaction logic for ShellWindow.xaml
    /// </summary>
    public partial class ShellWindow : Window
    {
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
            Application.Current.Shutdown();
        }
        private void WindowConfiguration(Users users)
        {
            _user = users;
            
        }
        #endregion
        private void MainNavigationView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            
        }

        private void AutoSuggestBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {

        }
    }
}
