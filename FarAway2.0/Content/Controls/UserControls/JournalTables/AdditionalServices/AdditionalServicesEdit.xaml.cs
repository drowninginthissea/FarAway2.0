using Microsoft.EntityFrameworkCore;
using ModernWpf.Controls;
using System.Windows.Controls;

namespace FarAway2._0.Content.Controls.UserControls.JournalTables.AdditionalServices
{
    public partial class AdditionalServicesEdit : UserControl, IContentDialogParent
    {
        public ContentDialog ParentDialog { get; set; }
        private bool _isAddition = true;
        AdditionalServicesForRent ChangingInstance;
        Func<Task> UpdateMethod;
        public AdditionalServicesEdit(ContentDialog CallingDialog, Func<Task> UpdateMethod)
        {
            InitializeComponent();
            ParentDialog = CallingDialog;
            this.UpdateMethod = UpdateMethod;
            FillComboBoxes();
        }
        public AdditionalServicesEdit(ContentDialog CallingDialog, Func<Task> UpdateMethod, AdditionalServicesForRent Service)
        {
            InitializeComponent();
            ParentDialog = CallingDialog;
            this.UpdateMethod = UpdateMethod;
            _isAddition = false;
            ChangingInstance = Service;
            FillComboBoxesAndFields(Service);
        }
        private async Task FillComboBoxes()
        {
            if (_isAddition)
                RentalCB.ItemsSource = await DbUtils.db.ParkingSpaceRental.Where(r => r.idRentalStatus == Entities.Enums.RentalStatuses.Active).ToListAsync();
            else
                RentalCB.ItemsSource = await DbUtils.db.ParkingSpaceRental.Where(r => r.idRentalStatus == Entities.Enums.RentalStatuses.Active || r == ChangingInstance.idRentNavigation).ToListAsync();
            
            ServiceCB.ItemsSource = await DbUtils.db.ListOfAdditionalServices.ToListAsync();
            FrequencyCB.ItemsSource = await DbUtils.db.FrequencyOfServices.ToListAsync();
            ProviderCB.ItemsSource = await DbUtils.db.ServiceProviders.ToListAsync();
        }
        private async Task FillComboBoxesAndFields(AdditionalServicesForRent Service)
        {
            await FillComboBoxes();
            RentalCB.SelectedItem = Service.idRentNavigation;
            RentalCB.IsEnabled = false;
            ServiceCB.SelectedItem = Service.idServiceNavigation;
            ServiceCB.IsEnabled = false;
            FrequencyCB.SelectedItem = Service.FrequencyOfServicePerformanceInDaysNavigation;
            ProviderCB.SelectedItem = Service.idServiceProvidersNavigation;
        }

        private void SaveChanges_Click(object sender, RoutedEventArgs e)
        {
            if (RentalCB.SelectedIndex == -1)
            {
                MessageBox.Show("Значение аренды для дополнительной услуги должно быть выбрано!", "Ошибка");
                return;
            }
            if (ServiceCB.SelectedIndex == -1)
            {
                MessageBox.Show("Значение дополнительной услуги должно быть выбрано!", "Ошибка");
                return;
            }
            if (FrequencyCB.SelectedIndex == -1)
            {
                MessageBox.Show("Значение частоты оказания услуги должно быть выбрано!", "Ошибка");
                return;
            }
            if (ProviderCB.SelectedIndex == -1)
            {
                MessageBox.Show("Значение поставщика услуги должно быть выбрано!", "Ошибка");
                return;
            }
            if (_isAddition)
            {
                AdditionalServicesForRent Instance = new AdditionalServicesForRent()
                {
                    idRent = (RentalCB.SelectedItem as ParkingSpaceRental).id,
                    idService = (ServiceCB.SelectedItem as ListOfAdditionalServices).id,
                    FrequencyOfServicePerformanceInDays = (FrequencyCB.SelectedItem as FrequencyOfServices).id,
                    idServiceProviders = (ProviderCB.SelectedItem as ServiceProviders).id
                };
                DbUtils.db.AdditionalServicesForRent.Add(Instance);
            }
            else
            {
                ChangingInstance.idRent = (RentalCB.SelectedItem as ParkingSpaceRental).id;
                ChangingInstance.idService = (ServiceCB.SelectedItem as ListOfAdditionalServices).id;
                ChangingInstance.FrequencyOfServicePerformanceInDays = (FrequencyCB.SelectedItem as FrequencyOfServices).id;
                ChangingInstance.idServiceProviders = (ProviderCB.SelectedItem as ServiceProviders).id;
            }
            DbUtils.db.SaveChanges();
            UpdateMethod();
            this.CloseContentDialog();
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            this.CloseContentDialog();
        }
    }
}
