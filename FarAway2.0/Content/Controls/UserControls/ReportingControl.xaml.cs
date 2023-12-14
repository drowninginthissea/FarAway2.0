using FarAway2._0.Tools.Reporting;
using Microsoft.EntityFrameworkCore;
using System.Windows.Controls;

namespace FarAway2._0.Content.Controls.UserControls
{
    public partial class ReportingControl : UserControl
    {
        public ReportingControl()
        {
            InitializeComponent();
            FillComboBoxes();
        }
        private async void FillComboBoxes()
        {
            RentalsCB.ItemsSource = await DbUtils.db.ParkingSpaceRental.ToListAsync();
        }

        private async void RentalContractGenerationButton_Click(object sender, RoutedEventArgs e)
        {
            if (RentalsCB.SelectedIndex == -1)
            {
                MessageBox.Show("Сначала необходимо выбрать аренду для создания договора", "Ошибка");
                return;
            }
            ParkingSpaceRental rent = RentalsCB.SelectedItem as ParkingSpaceRental;
            MainGrid.IsEnabled = false;
            await ReportGeneration.DoARentContract(rent);
            MainGrid.IsEnabled = true;
        }
    }
}
