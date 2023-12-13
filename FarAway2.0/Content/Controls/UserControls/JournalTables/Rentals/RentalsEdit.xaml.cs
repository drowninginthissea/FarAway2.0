using Azure;
using Microsoft.EntityFrameworkCore;
using ModernWpf.Controls;
using System.Windows.Controls;

namespace FarAway2._0.Content.Controls.UserControls.JournalTables.Rentals
{
    public partial class RentalsEdit : UserControl, IContentDialogParent
    {
        private DateTime? _startDate;
        private DateTime? _endDate;
        private decimal? _calculatedCost;
        private int _oldIdStatus;
        public ContentDialog ParentDialog { get; set; }
        private bool _isAddition = true;
        ParkingSpaceRental ChangingInstance;
        Func<Task> UpdateMethod;
        public RentalsEdit(ContentDialog CallingDialog, Func<Task> UpdateMethod)
        {
            InitializeComponent();
            ParentDialog = CallingDialog;
            this.UpdateMethod = UpdateMethod;
            FillComboBoxes();
            _ = FillComboBoxesWithAction(() =>
            {
                StatusCB.SelectedItem = 
                    DbUtils.db.RentalStatuses.Single(s => s.id == Entities.Enums.RentalStatuses.Active);
            });
        }
        public RentalsEdit(ContentDialog CallingDialog, Func<Task> UpdateMethod, ParkingSpaceRental Rental)
        {
            InitializeComponent();
            ParentDialog = CallingDialog;
            this.UpdateMethod = UpdateMethod;
            _isAddition = false;
            ChangingInstance = Rental;
            _startDate = Rental.RentalStartDate;
            _endDate = Rental.RentEndDate;
            _calculatedCost = Rental.TotalPrice;
            _oldIdStatus = Rental.idRentalStatus;
            _ = FillComboBoxesWithAction(Rental, (Rental) =>
            {
                ClientCB.SelectedItem = Rental.idUserNavigation;
                ParkingSpotCB.SelectedItem = Rental.idParkingSpotNavigation.idBranchNavigation;
                ParkingSpotCB.IsEnabled = false;
                StartDate.SelectedDate = Rental.RentalStartDate;
                EndDate.SelectedDate = Rental.RentEndDate;
                TypeOfRentalByDurationCB.SelectedItem = Rental.idTypeOfRentByDurationNavigation;
                TotalPriceTB.Text = $"{Rental.TotalPrice:0.00}";
                StatusCB.SelectedItem = Rental.idRentalStatusNavigation;
            });
        }
        private async Task FillComboBoxesWithAction(Action method)
        {
            await FillComboBoxes();
            method.Invoke();
        }
        private async Task FillComboBoxesWithAction(ParkingSpaceRental Rental,
            Action<ParkingSpaceRental> method)
        {
            await FillComboBoxes(Rental.idParkingSpotNavigation);
            method.Invoke(Rental);
        }
        private async Task FillComboBoxes(Entities.ParkingSpots EditingSpot = null)
        {
            ClientCB.ItemsSource =
                await DbUtils
                .db
                .Users
                .Where(u => u.idRole == Entities.Enums.Roles.Client)
                .ToListAsync();
            ParkingSpotCB.ItemsSource =
                await DbUtils
                .db
                .ParkingSpots
                .Where(s => s.idParkingSpotStatus == Entities.Enums.ParkingSpotStatuses.Free ||
                s == EditingSpot)
                .Select(s => s.idBranchNavigation)
                .Distinct()
                .ToListAsync();
            TypeOfRentalByDurationCB.ItemsSource =
                await DbUtils.db.TypeOfRentByDuration.ToListAsync();
            StatusCB.ItemsSource =
                await DbUtils.db.RentalStatuses.ToListAsync();
        }
        private void CalculateDurationAndCost()
        {
            if (!_startDate.HasValue)
                return;
            if (!_endDate.HasValue)
                return;
            int DiffInDay = (int)(_endDate.Value - _startDate.Value).TotalDays + 1;
            switch (DiffInDay)
            {
                case int n when n >= 1 && n <= 5:
                    TypeOfRentalByDurationCB.SelectedItem =
                        DbUtils
                        .db
                        .TypeOfRentByDuration
                        .Single(t => t.id == Entities.Enums.TypeOfRentByDuration.Short);
                    break;
                case int n when n >= 6 && n <= 30:
                    TypeOfRentalByDurationCB.SelectedItem =
                        DbUtils
                        .db
                        .TypeOfRentByDuration
                        .Single(t => t.id == Entities.Enums.TypeOfRentByDuration.Medium);
                    break;
                default:
                    TypeOfRentalByDurationCB.SelectedItem =
                        DbUtils
                        .db
                        .TypeOfRentByDuration
                        .Single(t => t.id == Entities.Enums.TypeOfRentByDuration.Long);
                    break;
            }
            if (ParkingSpotCB.SelectedIndex == -1)
                return;
            Entities.Branches Branch = ParkingSpotCB.SelectedItem as Entities.Branches;
            _calculatedCost = DiffInDay *
                DbUtils.db.Branches.Single(
                    b => b.id == Branch.id).BranchCharacteristics
                .TheCostOfAParkingSpacePerDay *
                (TypeOfRentalByDurationCB.SelectedItem as TypeOfRentByDuration).PriceCoefficient;
            TotalPriceTB.Text = $"{_calculatedCost:0.00} ₽";
        }
        private void StartDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (StartDate.SelectedDate.HasValue)
            {
                EndDate.DisplayDateStart = StartDate.SelectedDate.Value;
                if (EndDate.SelectedDate.HasValue && EndDate.SelectedDate < StartDate.SelectedDate)
                    EndDate.SelectedDate = StartDate.SelectedDate;
                _startDate = StartDate.SelectedDate.Value;
                CalculateDurationAndCost();
            }
        }
        private void EndDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (EndDate.SelectedDate.HasValue)
            {
                _endDate = EndDate.SelectedDate.Value;
                CalculateDurationAndCost();
            }
        }
        private void ParkingSpotCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CalculateDurationAndCost();
        }
        private void SaveChanges_Click(object sender, RoutedEventArgs e)
        {
            if (ClientCB.SelectedIndex == -1)
            {
                MessageBox.Show("Пользователь аренды должен быть выбран!", "Ошибка");
                return;
            }
            if (ParkingSpotCB.SelectedIndex == -1)
            {
                MessageBox.Show("Адрес парковочного места должен быть выбран!", "Ошибка");
                return;
            }
            if (!StartDate.SelectedDate.HasValue)
            {
                MessageBox.Show("Дата начала аренды должна быть выбрана!", "Ошибка");
                return;
            }
            if (StatusCB.SelectedIndex == -1)
            {
                MessageBox.Show("Статус аренды должен быть выбран!", "Ошибка");
                return;
            }
            if (!_endDate.HasValue && _startDate > DateTime.Now)
            {
                MessageBox.Show("При невыборе даты окончания аренды дата начала не может превышать текущую дату!", "Ошибка");
                return;
            }
            if (_isAddition)
            {
                Entities.Branches Branch = ParkingSpotCB.SelectedItem as Entities.Branches;
                int SpotId = DbUtils
                    .db
                    .ParkingSpots
                    .Where(s => s.idParkingSpotStatus == Entities.Enums.ParkingSpotStatuses.Free &&
                        s.idBranch == Branch.id)
                    .Min(s => s.id);
                ParkingSpaceRental Instance = new ParkingSpaceRental()
                {
                    idTypeOfRentByDuration = GetTypeByDurationWhenNull().id,
                    idUser = (ClientCB.SelectedItem as Entities.Users).id,
                    idParkingSpot = SpotId,
                    RentalStartDate = _startDate.Value,
                    RentEndDate = (_endDate.HasValue ? _endDate : null),
                    TotalPrice = (_calculatedCost.HasValue ? _calculatedCost : null),
                    idRentalStatus = (StatusCB.SelectedItem as RentalStatuses).id
                };
                if ((StatusCB.SelectedItem as RentalStatuses).id == Entities.Enums.RentalStatuses.Active)
                {
                    DbUtils.db.ParkingSpots.Single(s => s.id == SpotId).idParkingSpotStatus =
                        Entities.Enums.ParkingSpotStatuses.Busy;
                }
                DbUtils.db.ParkingSpaceRental.Add(Instance);
            }
            else
            {
                int NewIdStatus = (StatusCB.SelectedItem as RentalStatuses).id;
                ChangingInstance.idTypeOfRentByDuration =
                    (_endDate.HasValue ?
                    (TypeOfRentalByDurationCB.SelectedItem as TypeOfRentByDuration).id :
                    GetTypeByDurationWhenNull().id);
                ChangingInstance.idUser = (ClientCB.SelectedItem as Entities.Users).id;
                ChangingInstance.RentalStartDate = StartDate.SelectedDate.Value;
                ChangingInstance.RentEndDate = (_endDate.HasValue ? _endDate : null);
                ChangingInstance.TotalPrice = _calculatedCost;
                ChangingInstance.idRentalStatus = NewIdStatus;

                if (NewIdStatus != _oldIdStatus && _oldIdStatus == Entities.Enums.RentalStatuses.Active &&
                    (NewIdStatus == Entities.Enums.RentalStatuses.Completed ||
                     NewIdStatus == Entities.Enums.RentalStatuses.Canceled))
                    ChangingInstance.idParkingSpotNavigation.idParkingSpotStatus =
                        Entities.Enums.ParkingSpotStatuses.Free;
            }
            DbUtils.db.SaveChanges();
            UpdateMethod();
            this.CloseContentDialog();
        }
        private TypeOfRentByDuration GetTypeByDurationWhenNull()
        {
            DateTime SecondTempDate = DateTime.Now;
            int DiffInDay = (int)(SecondTempDate - _startDate.Value).TotalDays + 1;
            switch (DiffInDay)
            {
                case int n when n >= 1 && n <= 5:
                    return DbUtils.db.TypeOfRentByDuration
                        .Single(t => t.id == Entities.Enums.TypeOfRentByDuration.Short);
                case int n when n >= 6 && n <= 30:
                    return DbUtils.db.TypeOfRentByDuration
                        .Single(t => t.id == Entities.Enums.TypeOfRentByDuration.Medium);
                default:
                    return DbUtils.db.TypeOfRentByDuration
                        .Single(t => t.id == Entities.Enums.TypeOfRentByDuration.Long);
            }
        }
        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            this.CloseContentDialog();
        }
    }
}