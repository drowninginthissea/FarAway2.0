using ModernWpf.Controls;
using System.Windows.Controls;

namespace FarAway2._0.Content.Controls.UserControls.ReferenceTables.DataEdit
{
    public partial class ParkingSpotStatusesEdit : UserControl, IContentDialogParent
    {
        public ContentDialog ParentDialog { get; set; }
        private bool _isAddition = true;
        ParkingSpotStatuses ChangingInstance;
        Func<Task> UpdateMethod;
        public ParkingSpotStatusesEdit(ContentDialog CallingDialog, Func<Task> UpdateMethod)
        {
            InitializeComponent();
            ParentDialog = CallingDialog;
            this.UpdateMethod = UpdateMethod;
        }
        public ParkingSpotStatusesEdit(ContentDialog CallingDialog, ParkingSpotStatuses Instance, Func<Task> UpdateMethod)
        {
            InitializeComponent();
            ParentDialog = CallingDialog;
            this.UpdateMethod = UpdateMethod;
            _isAddition = false;
            ChangingInstance = Instance;
            StatusName.Text = Instance.StatusName;
        }

        private void SaveChanges_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(StatusName.Text))
            {
                MessageBox.Show("Значение названия статуса не может быть пустым!", "Ошибка сохранения");
                return;
            }
            if (StatusName.Text.Length > 30)
            {
                MessageBox.Show("Длина записи не может превышать 30 символов!", "Ошибка сохранения");
                return;
            }
            if (_isAddition)
            {
                ParkingSpotStatuses Instance = new ParkingSpotStatuses()
                {
                    StatusName = StatusName.Text
                };
                DbUtils.db.ParkingSpotStatuses.Add(Instance);
            }
            else
            {
                ChangingInstance.StatusName = StatusName.Text;
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
