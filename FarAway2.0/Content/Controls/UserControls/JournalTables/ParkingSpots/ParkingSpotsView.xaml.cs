using FarAway2._0.BaseClasses;
using Microsoft.EntityFrameworkCore;
using ModernWpf.Controls;
using System.Windows.Controls;

namespace FarAway2._0.Content.Controls.UserControls.JournalTables.ParkingSpots
{
    public partial class ParkingSpotsView : SearchableTableView
    {
        
        public ParkingSpotsView()
        {
            InitializeComponent();
        }
        public override async Task UpdateDataAsync()
        {
            MainDataGrid.ItemsSource = (await DbUtils.db.ParkingSpots.ToListAsync())
                .Where(s => s.idBranchNavigation.Address.Contains(TextToSearch) ||
                s.idParkingSpotStatusNavigation.StatusName.Contains(TextToSearch) ||
                DbUtils.GetInitialsOfDefaultForParkingSpots(
                    DbUtils.GetLastUserRental(s.ParkingSpaceRental)).Contains(TextToSearch)
                    );
        }
        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            ContentDialog DialogOfEdit = new ContentDialog();
            ParkingSpotsEdit Control = new ParkingSpotsEdit(DialogOfEdit, UpdateDataAsync);
            StaticTools.OpenDialog(DialogOfEdit, Control);
        }
        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            Entities.ParkingSpots Spot = (sender as Button).DataContext as Entities.ParkingSpots;
            ContentDialog DialogOfEdit = new ContentDialog();
            ParkingSpotsEdit Control = new ParkingSpotsEdit(DialogOfEdit, UpdateDataAsync, Spot);
            StaticTools.OpenDialog(DialogOfEdit, Control);
        }
    }
}
