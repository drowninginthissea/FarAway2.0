using FarAway2._0.BaseClasses;
using FarAway2._0.Content.Controls.UserControls.JournalTables.ParkingSpots;
using FarAway2._0.Converters;
using Microsoft.EntityFrameworkCore;
using ModernWpf.Controls;
using System.Windows.Controls;

namespace FarAway2._0.Content.Controls.UserControls.JournalTables.Rentals
{
    public partial class RentalsView : SearchableTableView
    {
        public RentalsView()
        {
            InitializeComponent();
        }
        public override async Task UpdateDataAsync()
        {
            MainDataGrid.ItemsSource = (await DbUtils.db.ParkingSpaceRental.ToListAsync())
                .Where(r =>  $"{r.RentalStartDate:dd.MM.yyyy}".Contains(TextToSearch) ||
                (r.RentEndDate != null ? $"{r.RentEndDate:dd.MM.yyyy}" : "Дата окончания не определена").Contains(TextToSearch) ||
                r.idParkingSpotNavigation.idBranchNavigation.Address.Contains(TextToSearch) ||
                $"{r.idTypeOfRentByDurationNavigation.TypeName}({(r.idTypeOfRentByDurationNavigation.MaxDurationOfRentalDays == null ? $"от 31 дня" : $"от {r.idTypeOfRentByDurationNavigation.MinDurationOfRentalDays} до {r.idTypeOfRentByDurationNavigation.MaxDurationOfRentalDays} дней")})".Contains(TextToSearch) ||
                DbUtils.GetUserInitials(r.idUserNavigation).Contains(TextToSearch) ||
                (r.TotalPrice != null ? $"{r.TotalPrice:0.00}" : "Ещё не рассчитана").Contains(TextToSearch) ||
                r.idRentalStatusNavigation.StatusName.Contains(TextToSearch) ||
                (r.AdditionalServicesForRent.Count == 0 ? "Нет дополнительных услуг" : AdditionalServicesConverter.GetAddedServices(r.AdditionalServicesForRent)).Contains(TextToSearch));
        }
        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            ContentDialog DialogOfEdit = new ContentDialog();
            RentalsEdit Control = new RentalsEdit(DialogOfEdit, UpdateDataAsync);
            StaticTools.OpenDialog(DialogOfEdit, Control);
        }
        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            ParkingSpaceRental Rental = (sender as Button).DataContext as ParkingSpaceRental;
            ContentDialog Dialog = new ContentDialog();
            RentalsEdit Control = new RentalsEdit(Dialog, UpdateDataAsync, Rental);
            StaticTools.OpenDialog(Dialog, Control);
        }
    }
}
