using FarAway2._0.Content.Controls.UserControls.DataEdit;
using FarAway2._0.Entities.Enums;
using FarAway2._0.Interfaces;
using FarAway2._0.Tools.Extensions;
using Microsoft.EntityFrameworkCore;
using ModernWpf.Controls;
using System.Collections.Generic;
using System.Windows.Controls;

namespace FarAway2._0.Content.Controls.UserControls
{
    public partial class ReferenceTablesView : UserControl, ISearchable
    {
        private TableNames _activeReference;
        private string _textToSearch;
        public string TextToSearch
        {
            get { return _textToSearch; }
            set
            {
                if (_textToSearch != value)
                {
                    _textToSearch = value;
                    UpdateGridAsync();
                }
            }
        }
        public ReferenceTablesView(TableNames table)
        {
            InitializeComponent();

            _activeReference = table;
        }

        private void ShowDesiredDataGrid<T>(DataGrid Grid, List<T> CollectionToShow)
        {
            Grid.Visibility = Visibility.Visible;
            Grid.ItemsSource = CollectionToShow.SearchData(TextToSearch);
        }

        private async Task UpdateGridAsync()
        {
            switch (_activeReference)
            {
                case TableNames.FrequencyOfServices:
                    ShowDesiredDataGrid(FrequencyOfServicesGrid,
                        await DbUtils.db.FrequencyOfServices.ToListAsync());
                    break;
                case TableNames.ListOfActions:
                    ShowDesiredDataGrid(ListOfActionsGrid,
                        await DbUtils.db.ListOfActions.ToListAsync());
                    break;
                case TableNames.ListOfAdditionalServices:
                    ShowDesiredDataGrid(ListOfAdditionalServicesGrid,
                        await DbUtils.db.ListOfAdditionalServices.ToListAsync());
                    break;
                case TableNames.ParkingSpotStatuses:
                    ShowDesiredDataGrid(ParkingSpotStatusesGrid,
                        await DbUtils.db.ParkingSpotStatuses.ToListAsync());
                    break;
                case TableNames.RentalStatuses:
                    ShowDesiredDataGrid(RentalStatusesGrid,
                        await DbUtils.db.RentalStatuses.ToListAsync());
                    break;
                case TableNames.Roles:
                    ShowDesiredDataGrid(RolesGrid,
                        await DbUtils.db.Roles.ToListAsync());
                    break;
                case TableNames.ServiceProviders:
                    ShowDesiredDataGrid(ServiceProvidersGrid,
                        await DbUtils.db.ServiceProviders.ToListAsync());
                    break;
                case TableNames.TypeOfRentByDuration:
                    ShowDesiredDataGrid(TypeOfRentByDurationGrid,
                        await DbUtils.db.TypeOfRentByDuration.ToListAsync());
                    break;
                case TableNames.TypesOfCarExchangeSystem:
                    ShowDesiredDataGrid(TypesOfCarExchangeSystemGrid,
                        await DbUtils.db.TypesOfCarExchangeSystem.ToListAsync());
                    break;
                case TableNames.TypesOfParking:
                    ShowDesiredDataGrid(TypesOfParkingGrid,
                        await DbUtils.db.TypesOfParking.ToListAsync());
                    break;
            }
        }

        /// <summary>
        /// Call this method together with the constructor and wait for its result
        /// </summary>
        public async Task LoadDataAsync()
        {
            await UpdateGridAsync();
        }

        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void FrequencyOfServicesEditButton_Click(object sender, RoutedEventArgs e)
        {
            FrequencyOfServices Instance = (sender as Button).DataContext as FrequencyOfServices;
            ContentDialog DialogOfEdit = new ContentDialog();
            FrequencyOfServicesEdit EditControl = new FrequencyOfServicesEdit(DialogOfEdit);
            DialogOfEdit.Content = EditControl;
            DialogOfEdit.ShowAsync();
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
            new ContentDialog() { Content = new ServiceProvidersEdit() }.ShowAsync();
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
