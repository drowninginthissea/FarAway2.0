using ModernWpf.Controls;
using System.Windows.Controls;

namespace FarAway2._0.Content.Controls.UserControls.ReferenceTables.DataEdit
{
    public partial class ListOfAdditionalServicesEdit : UserControl, IContentDialogParent
    {
        public ContentDialog ParentDialog { get; set; }
        private bool _isAddition = true;
        ListOfAdditionalServices ChangingInstance;
        Func<Task> UpdateMethod;
        public ListOfAdditionalServicesEdit(ContentDialog CallingDialog, Func<Task> UpdateMethod)
        {
            InitializeComponent();
            ParentDialog = CallingDialog;
            this.UpdateMethod = UpdateMethod;
        }
        public ListOfAdditionalServicesEdit(ContentDialog CallingDialog, ListOfAdditionalServices Instance, Func<Task> UpdateMethod)
        {
            InitializeComponent();
            ParentDialog = CallingDialog;
            this.UpdateMethod = UpdateMethod;
            _isAddition = false;
            ChangingInstance = Instance;
            ServiceName.Text = Instance.ServiceName;
            ServicePrice.Text = $"{Instance.ServicePrice:0.00}";
            ServiceDescription.Text = Instance.ServiceDescription;
        }

        private void SaveChanges_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(ServiceName.Text))
            {
                MessageBox.Show("Значение названия услуги не может быть пустым!", "Ошибка сохранения");
                return;
            }
            if (!decimal.TryParse(ServicePrice.Text, out decimal ParsedValue) ||
                !DbUtils.ValidateDecimal(ParsedValue, 10, 2))
            {
                MessageBox.Show("Значение цены услуги введено не корректно!", "Ошибка сохранения");
                return;
            }
            if (string.IsNullOrWhiteSpace(ServiceDescription.Text))
            {
                MessageBox.Show("Значение описания услуги не может быть пустым!", "Ошибка сохранения");
                return;
            }

            if (_isAddition)
            {
                ListOfAdditionalServices Instance = new ListOfAdditionalServices()
                {
                    ServiceName = ServiceName.Text,
                    ServicePrice = ParsedValue,
                    ServiceDescription = ServiceDescription.Text
                };
                DbUtils.db.ListOfAdditionalServices.Add(Instance);
            }
            else
            {
                ChangingInstance.ServiceName = ServiceName.Text;
                ChangingInstance.ServicePrice = ParsedValue;
                ChangingInstance.ServiceDescription = ServiceDescription.Text;
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
