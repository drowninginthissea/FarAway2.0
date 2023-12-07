using ModernWpf.Controls;
using System.Windows.Controls;

namespace FarAway2._0.Content.Controls.UserControls.ReferenceTables.DataEdit
{
    public partial class RentalStatusesEdit : UserControl, IContentDialogParent
    {
        public ContentDialog ParentDialog { get; set; }
        private bool _isAddition = true;
        RentalStatuses ChangingInstance;
        Func<Task> UpdateMethod;
        public RentalStatusesEdit(ContentDialog CallingDialog, Func<Task> UpdateMethod)
        {
            InitializeComponent();
            ParentDialog = CallingDialog;
            this.UpdateMethod = UpdateMethod;
        }
        public RentalStatusesEdit(ContentDialog CallingDialog, RentalStatuses Instance, Func<Task> UpdateMethod)
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
            if (StatusName.Text.Length > 20)
            {
                MessageBox.Show("Длина записи не может превышать 20 символов!", "Ошибка сохранения");
                return;
            }
            if (_isAddition)
            {
                RentalStatuses Instance = new RentalStatuses()
                {
                    StatusName = StatusName.Text
                };
                DbUtils.db.RentalStatuses.Add(Instance);
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
