using FarAway2._0.Entities.Enums;
using FarAway2._0.Content.Controls.UserControls.DataEdit;
using System.Windows.Controls;
using ModernWpf.Controls;
using System.Threading;

namespace FarAway2._0.Content.Controls.UserControls
{
    public partial class ReferenceTablesView : UserControl
    {
        private TableNames _activeReference;
        public ReferenceTablesView(TableNames table)
        {
            InitializeComponent();

        }

        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            //FrequencyOfServicesGrid.Visibility = Visibility.Visible;
            //FrequencyOfServicesGrid.ItemsSource = DbUtils.db.FrequencyOfServices.ToList();
        }

        private void FrequencyOfServicesEditButton_Click(object sender, RoutedEventArgs e)
        {
            //DataEdit.Content = new FrequencyOfServicesEdit();
            //DataEdit.Visibility = Visibility.Visible;
            //FrequencyOfServices some = (sender as Button).DataContext as FrequencyOfServices;
            new ContentDialog() { Content = new FrequencyOfServicesEdit() }.ShowAsync();
        }

        private void ListOfActionsEditButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ListOfAdditionalServicesEditButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ParkingSpotStatusesEditButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void RentalStatusesEditButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void RolesEditButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ServiceProvidersEditButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void TypeOfRentByDurationEditButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void TypesOfCarExchangeSystemEditButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void TypesOfParkingEditButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
