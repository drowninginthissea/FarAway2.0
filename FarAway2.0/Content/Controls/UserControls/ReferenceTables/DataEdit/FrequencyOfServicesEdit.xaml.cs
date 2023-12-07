using ModernWpf.Controls;
using System.Windows.Controls;

namespace FarAway2._0.Content.Controls.UserControls.ReferenceTables.DataEdit
{
    public partial class FrequencyOfServicesEdit : UserControl, IContentDialogParent
    {
        public ContentDialog ParentDialog { get; set; }
        private bool _isAddition = true;
        FrequencyOfServices ChangingInstance;
        Func<Task> UpdateMethod;
        public FrequencyOfServicesEdit(ContentDialog CallingDialog, Func<Task> UpdateMethod)
        {
            InitializeComponent();
            ParentDialog = CallingDialog;
            this.UpdateMethod = UpdateMethod;
        }
        public FrequencyOfServicesEdit(ContentDialog CallingDialog, FrequencyOfServices Instance, Func<Task> UpdateMethod)
        {
            InitializeComponent();
            ParentDialog = CallingDialog;
            this.UpdateMethod = UpdateMethod;
            _isAddition = false;
            ChangingInstance = Instance;
            FrequencyName.Text = Instance.FrequencyName;
        }

        private void SaveChanges_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(FrequencyName.Text))
            {
                MessageBox.Show("Значение названия периодичности не может быть пустым!", "Ошибка сохранения");
                return;
            }
            if (FrequencyName.Text.Length > 50)
            {
                MessageBox.Show("Длина записи не может превышать 50 символов!", "Ошибка сохранения");
                return;
            }
            if (_isAddition)
            {
                FrequencyOfServices Instance = new FrequencyOfServices()
                {
                    FrequencyName = FrequencyName.Text
                };
                DbUtils.db.FrequencyOfServices.Add(Instance);
            }
            else
            {
                ChangingInstance.FrequencyName = FrequencyName.Text;
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
