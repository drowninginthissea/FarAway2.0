using FarAway2._0.BaseClasses;
using FarAway2._0.Content.Windows;
using Microsoft.EntityFrameworkCore;
using ModernWpf.Controls;
using System.Windows.Controls;

namespace FarAway2._0.Content.Controls.UserControls.JournalTables.Branches
{
    public partial class BranchesView : SearchableTableView
    {
        public BranchesView()
        {
            InitializeComponent();
        }
        public override async Task UpdateDataAsync()
        {
            MainDataGrid.ItemsSource = (await DbUtils.db.Branches.ToListAsync())
                .Where(b => b.Address.Contains(TextToSearch) ||
                b.idTypeOfParkingNavigation.TypeName.Contains(TextToSearch) ||
                b.idTypeOfCarExchangeSystemNavigation.TypeName.Contains(TextToSearch) ||
                b.BranchCharacteristics.CountOfParkingSpaces.ToString().Contains(TextToSearch) ||
                $"{b.BranchCharacteristics.TheCostOfAParkingSpacePerDay:0.00}".Contains(TextToSearch) ||
                b.BranchCharacteristics.WidthOfTheLiftingAndLoweringMechanismInMillimeters.ToString().Contains(TextToSearch) ||
                b.BranchCharacteristics.TotalWidthOfTheCarInMillimeters.ToString().Contains(TextToSearch) ||
                b.BranchCharacteristics.MaximumCarLengthInMillimeters.ToString().Contains(TextToSearch) ||
                b.BranchCharacteristics.MaximumHeightOfTheVehicleInMillimeters.ToString().Contains(TextToSearch) ||
                b.BranchCharacteristics.MaximumVehicleWeightInKilograms.ToString().Contains(TextToSearch));
        }
        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            ContentDialog DialogOfEdit = new ContentDialog();
            BranchesEdit Control = new BranchesEdit(DialogOfEdit, UpdateDataAsync);
            StaticTools.OpenDialog(DialogOfEdit, Control);
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            Entities.Branches Instance = (sender as Button).DataContext as Entities.Branches;
            ContentDialog DialogOfEdit = new ContentDialog();
            BranchesEdit Control = new BranchesEdit(DialogOfEdit, UpdateDataAsync, Instance);
            StaticTools.OpenDialog(DialogOfEdit, Control);
        }
    }
}
