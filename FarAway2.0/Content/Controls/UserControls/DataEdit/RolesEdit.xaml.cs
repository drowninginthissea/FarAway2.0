using ModernWpf.Controls;
using System.Windows.Controls;

namespace FarAway2._0.Content.Controls.UserControls.DataEdit
{
    public partial class RolesEdit : UserControl, IContentDialogParent
    {
        public ContentDialog ParentDialog { get; set; }
        private bool _isAddition = true;
        Roles ChangingInstance;
        Func<Task> UpdateMethod;
        public RolesEdit(ContentDialog CallingDialog, Func<Task> UpdateMethod)
        {
            InitializeComponent();
            ParentDialog = CallingDialog;
            this.UpdateMethod = UpdateMethod;
        }
        public RolesEdit(ContentDialog CallingDialog, Roles Instance, Func<Task> UpdateMethod)
        {
            InitializeComponent();
            ParentDialog = CallingDialog;
            this.UpdateMethod = UpdateMethod;
            _isAddition = false;
            ChangingInstance = Instance;
            RoleName.Text = Instance.RoleName;
        }

        private void SaveChanges_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(RoleName.Text))
            {
                MessageBox.Show("Значение названия роли не может быть пустым!", "Ошибка сохранения");
                return;
            }
            if (RoleName.Text.Length > 30)
            {
                MessageBox.Show("Длина записи не может превышать 30 символов!", "Ошибка сохранения");
                return;
            }
            if (_isAddition)
            {
                Roles Instance = new Roles()
                {
                    RoleName = RoleName.Text
                };
                DbUtils.db.Roles.Add(Instance);
            }
            else
            {
                ChangingInstance.RoleName = RoleName.Text;
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
