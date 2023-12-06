using ModernWpf.Controls;
using System.Windows.Controls;

namespace FarAway2._0.Content.Controls.UserControls.DataEdit
{
    public partial class ListOfActionsEdit : UserControl, IContentDialogParent
    {
        public ContentDialog ParentDialog { get; set; }
        private bool _isAddition = true;
        ListOfActions ChangingInstance;
        Func<Task> UpdateMethod;
        public ListOfActionsEdit(ContentDialog CallingDialog, Func<Task> UpdateMethod)
        {
            InitializeComponent();
            ParentDialog = CallingDialog;
            this.UpdateMethod = UpdateMethod;
        }
        public ListOfActionsEdit(ContentDialog CallingDialog, ListOfActions Instance, Func<Task> UpdateMethod)
        {
            InitializeComponent();
            ParentDialog = CallingDialog;
            this.UpdateMethod = UpdateMethod;
            _isAddition = false;
            ChangingInstance = Instance;
            ActionName.Text = Instance.ActionName;
        }

        private void SaveChanges_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(ActionName.Text))
            {
                MessageBox.Show("Значение названия действия не может быть пустым!", "Ошибка сохранения");
                return;
            }
            if (_isAddition)
            {
                ListOfActions Instance = new ListOfActions()
                {
                    ActionName = ActionName.Text
                };
                DbUtils.db.ListOfActions.Add(Instance);
            }
            else
            {
                ChangingInstance.ActionName = ActionName.Text;
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
