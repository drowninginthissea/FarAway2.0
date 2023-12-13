using FarAway2._0.BaseClasses;
using FarAway2._0.Converters;
using Microsoft.EntityFrameworkCore;
using ModernWpf.Controls;
using System.Windows.Controls;

namespace FarAway2._0.Content.Controls.UserControls.JournalTables.AdditionalServices
{
    public partial class AdditionalServicesView : SearchableTableView
    {
        public AdditionalServicesView()
        {
            InitializeComponent();
        }
        public override async Task UpdateDataAsync()
        {
            MainDataGrid.ItemsSource = (await DbUtils.db.AdditionalServicesForRent.ToListAsync())
                .Where(s => AddedServicesRentalConverter.ConvertRental(s.idRentNavigation).Contains(TextToSearch) ||
                FullServiceConverter.ConvertService(s.idServiceNavigation).Contains(TextToSearch) ||
                s.FrequencyOfServicePerformanceInDaysNavigation.FrequencyName.Contains(TextToSearch) ||
                s.idServiceProvidersNavigation.Name.Contains(TextToSearch));
        }

        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            ContentDialog Dialog = new ContentDialog();
            AdditionalServicesEdit Control = new AdditionalServicesEdit(Dialog, UpdateDataAsync);
            StaticTools.OpenDialog(Dialog, Control);

        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            AdditionalServicesForRent Instance = (sender as Button).DataContext as AdditionalServicesForRent;
            ContentDialog Dialog = new ContentDialog();
            AdditionalServicesEdit Control = new AdditionalServicesEdit(Dialog, UpdateDataAsync, Instance);
            StaticTools.OpenDialog(Dialog, Control);
        }
    }
}
