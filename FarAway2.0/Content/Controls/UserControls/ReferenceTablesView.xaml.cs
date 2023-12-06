using FarAway2._0.Content.Controls.UserControls.DataEdit;
using FarAway2._0.Entities.Enums;
using Microsoft.EntityFrameworkCore;
using ModernWpf.Controls;
using System;
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
        public async Task UpdateDataAsync()
        {
            await UpdateGridAsync();
        }

        public delegate ControlType DataCreateConstructor<ControlType>(ContentDialog CallingDialog, Func<Task> UpdateMethod);
        public delegate ControlType DataUpdateConstructor<EntityType, ControlType>
            (ContentDialog CallingDialog, EntityType Instance, Func<Task> UpdateMethod);


        public void OpenUpdateDialogGeneric<EntityType, ControlType>
            (object sender, DataUpdateConstructor<EntityType, ControlType> Constructor, Func<Task> UpdateMethod)
            where EntityType : class
            where ControlType : UserControl =>
            OpenDialogGeneric<EntityType, ControlType, DataUpdateConstructor<EntityType, ControlType>>
                (sender, Constructor, UpdateMethod);

        public void OpenCreateDialogGeneric<ControlType>
            (DataCreateConstructor<ControlType> Constructor, Func<Task> UpdateMethod, object sender = null)
            where ControlType : UserControl =>
            OpenDialogGeneric<object, ControlType, DataCreateConstructor<ControlType>>(sender, Constructor, UpdateMethod);

        private void OpenDialogGeneric<EntityType, ControlType, CtorMethod>
            (object sender, CtorMethod MethodReference, Func<Task> UpdateMethod)
            where EntityType : class
            where ControlType : UserControl
            where CtorMethod : Delegate
        {
            EntityType Instance = null;
            if (MethodReference is DataUpdateConstructor<EntityType, ControlType>)
                Instance = (sender as Button).DataContext as EntityType;
            ContentDialog DialogOfEdit = new ContentDialog();
            ControlType UserControl = null;
            if (MethodReference is DataUpdateConstructor<EntityType, ControlType> Update)
                UserControl = Update(DialogOfEdit, Instance, UpdateMethod);
            else if (MethodReference is DataCreateConstructor<ControlType> Create)
                UserControl = Create(DialogOfEdit, UpdateMethod);
            DialogOfEdit.Content = UserControl;
            DialogOfEdit.ShowAsync();
        }

        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            switch (_activeReference)
            {
                case TableNames.FrequencyOfServices:
                    OpenCreateDialogGeneric
                        ((callingDialog, method) => new FrequencyOfServicesEdit(callingDialog, method), UpdateDataAsync);
                    break;
                case TableNames.ListOfActions:
                    OpenCreateDialogGeneric
                        ((callingDialog, method) => new ListOfActionsEdit(callingDialog, method), UpdateDataAsync);
                    break;
                case TableNames.ListOfAdditionalServices:
                    OpenCreateDialogGeneric
                        ((callingDialog, method) => new ListOfAdditionalServicesEdit(callingDialog, method), UpdateDataAsync);
                    break;
                case TableNames.ParkingSpotStatuses:
                    OpenCreateDialogGeneric
                        ((callingDialog, method) => new ParkingSpotStatusesEdit(callingDialog, method), UpdateDataAsync);
                    break;
                case TableNames.RentalStatuses:
                    OpenCreateDialogGeneric
                        ((callingDialog, method) => new RentalStatusesEdit(callingDialog, method), UpdateDataAsync);
                    break;
                case TableNames.Roles:
                    OpenCreateDialogGeneric
                        ((callingDialog, method) => new RolesEdit(callingDialog, method), UpdateDataAsync);
                    break;
                case TableNames.ServiceProviders:
                    OpenCreateDialogGeneric
                        ((callingDialog, method) => new ServiceProvidersEdit(callingDialog, method), UpdateDataAsync);
                    break;
                case TableNames.TypeOfRentByDuration:
                    OpenCreateDialogGeneric
                        ((callingDialog, method) => new TypeOfRentByDurationEdit(callingDialog, method), UpdateDataAsync);
                    break;
                case TableNames.TypesOfCarExchangeSystem:
                    OpenCreateDialogGeneric
                        ((callingDialog, method) => new TypesOfCarExchangeSystemEdit(callingDialog, method), UpdateDataAsync);
                    break;
                case TableNames.TypesOfParking:
                    OpenCreateDialogGeneric
                        ((callingDialog, method) => new TypesOfParkingEdit(callingDialog, method), UpdateDataAsync);
                    break;
            }
        }

        private void FrequencyOfServicesEditButton_Click(object sender, RoutedEventArgs e) =>
            OpenUpdateDialogGeneric<FrequencyOfServices, FrequencyOfServicesEdit>
                (sender, 
                (callingDialog, instance, method) => new FrequencyOfServicesEdit(callingDialog, instance, method), 
                UpdateDataAsync);

        private void ListOfActionsEditButton_Click(object sender, RoutedEventArgs e) =>
            OpenUpdateDialogGeneric<ListOfActions, ListOfActionsEdit>
                (sender,
                (callingDialog, instance, method) => new ListOfActionsEdit(callingDialog, instance, method),
                UpdateDataAsync);

        private void ListOfAdditionalServicesEditButton_Click(object sender, RoutedEventArgs e) =>
            OpenUpdateDialogGeneric<ListOfAdditionalServices, ListOfAdditionalServicesEdit>
                (sender,
                (callingDialog, instance, method) => new ListOfAdditionalServicesEdit(callingDialog, instance, method),
                UpdateDataAsync);

        private void ParkingSpotStatusesEditButton_Click(object sender, RoutedEventArgs e) =>
            OpenUpdateDialogGeneric<ParkingSpotStatuses, ParkingSpotStatusesEdit>
                (sender,
                (callingDialog, instance, method) => new ParkingSpotStatusesEdit(callingDialog, instance, method),
                UpdateDataAsync);

        private void RentalStatusesEditButton_Click(object sender, RoutedEventArgs e) =>
            OpenUpdateDialogGeneric<RentalStatuses, RentalStatusesEdit>
                (sender,
                (callingDialog, instance, method) => new RentalStatusesEdit(callingDialog, instance, method),
                UpdateDataAsync);

        private void RolesEditButton_Click(object sender, RoutedEventArgs e) =>
            OpenUpdateDialogGeneric<Entities.Roles, RolesEdit>
                (sender,
                (callingDialog, instance, method) => new RolesEdit(callingDialog, instance, method),
                UpdateDataAsync);

        private void ServiceProvidersEditButton_Click(object sender, RoutedEventArgs e) =>
            OpenUpdateDialogGeneric<ServiceProviders, ServiceProvidersEdit>
                (sender,
                (callingDialog, instance, method) => new ServiceProvidersEdit(callingDialog, instance, method),
                UpdateDataAsync);

        private void TypeOfRentByDurationEditButton_Click(object sender, RoutedEventArgs e) =>
            OpenUpdateDialogGeneric<TypeOfRentByDuration, TypeOfRentByDurationEdit>
                (sender,
                (callingDialog, instance, method) => new TypeOfRentByDurationEdit(callingDialog, instance, method),
                UpdateDataAsync);

        private void TypesOfCarExchangeSystemEditButton_Click(object sender, RoutedEventArgs e) =>
            OpenUpdateDialogGeneric<TypesOfCarExchangeSystem, TypesOfCarExchangeSystemEdit>
                (sender,
                (callingDialog, instance, method) => new TypesOfCarExchangeSystemEdit(callingDialog, instance, method),
                UpdateDataAsync);

        private void TypesOfParkingEditButton_Click(object sender, RoutedEventArgs e) =>
            OpenUpdateDialogGeneric<TypesOfParking, TypesOfParkingEdit>
                (sender,
                (callingDialog, instance, method) => new TypesOfParkingEdit(callingDialog, instance, method),
                UpdateDataAsync);
    }
}
