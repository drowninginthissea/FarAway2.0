using Microsoft.EntityFrameworkCore;
using ModernWpf.Controls;
using System.Windows.Controls;

namespace FarAway2._0.Content.Controls.UserControls.JournalTables.ParkingSpots
{
    public partial class ParkingSpotsEdit : UserControl, IContentDialogParent
    {
        public ContentDialog ParentDialog { get; set; }
        private bool _isAddition = true;
        Entities.ParkingSpots ChangingInstance;
        Func<Task> UpdateMethod;
        public ParkingSpotsEdit(ContentDialog CallingDialog, Func<Task> UpdateMethod)
        {
            InitializeComponent();
            ParentDialog = CallingDialog;
            this.UpdateMethod = UpdateMethod;
            FillComboBoxes();
        }
        public ParkingSpotsEdit(ContentDialog CallingDialog, Func<Task> UpdateMethod, Entities.ParkingSpots Spot)
        {
            InitializeComponent();
            ParentDialog = CallingDialog;
            this.UpdateMethod = UpdateMethod;
            _isAddition = false;
            ChangingInstance = Spot;
            FillComboBoxes();
            BranchCB.SelectedItem = Spot.idBranchNavigation;
            StatusCB.SelectedItem = Spot.idParkingSpotStatusNavigation;
        }
        private async void FillComboBoxes()
        {
            BranchCB.ItemsSource = await DbUtils.db.Branches.ToListAsync();
            StatusCB.ItemsSource = await DbUtils.db.ParkingSpotStatuses.ToListAsync();
        }
        private void SaveChanges_Click(object sender, RoutedEventArgs e)
        {
            if (BranchCB.SelectedIndex == -1)
            {
                MessageBox.Show("Значение парковки должно быть выбрано!", "Ошибка сохранения");
                return;
            }
            if (StatusCB.SelectedIndex == -1)
            {
                MessageBox.Show("Значение статуса должно быть выбрано!", "Ошибка сохранения");
                return;
            }
            if (_isAddition)
            {
                Entities.ParkingSpots Instance = new Entities.ParkingSpots()
                {
                    idBranch = (BranchCB.SelectedItem as Entities.Branches).id,
                    idParkingSpotStatus = (StatusCB.SelectedItem as ParkingSpotStatuses).id
                };
                DbUtils.db.ParkingSpots.Add(Instance);
            }
            else
            {
                ChangingInstance.idBranch = (BranchCB.SelectedItem as Entities.Branches).id;
                ChangingInstance.idParkingSpotStatus = (StatusCB.SelectedItem as ParkingSpotStatuses).id;
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
