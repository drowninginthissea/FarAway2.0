using FarAway2._0.Entities.Enums;
using FarAway2._0.Content.Controls.UserControls.DataEdit;
using System.Windows.Controls;

namespace FarAway2._0.Content.Controls.UserControls
{
    public partial class ReferenceTablesView : UserControl
    {
        public ReferenceTablesView(TableNames table)
        {
            InitializeComponent();
        }

        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            FrequencyOfServicesGrid.Visibility = Visibility.Visible;
            FrequencyOfServicesGrid.ItemsSource = DbUtils.db.FrequencyOfServices.ToList();
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            DataEdit.Content = new FrequencyOfServicesEdit();
            MainGrid.IsEnabled = false;
            DataEdit.Visibility = Visibility.Visible;

        }
    }
}
