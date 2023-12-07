using ModernWpf.Controls;
using System.Windows.Controls;

namespace FarAway2._0.Content.Controls.UserControls.ReferenceTables.DataEdit
{
    public partial class TypesOfParkingEdit : UserControl, IContentDialogParent
    {
        public ContentDialog ParentDialog { get; set; }
        private bool _isAddition = true;
        TypesOfParking ChangingInstance;
        Func<Task> UpdateMethod;
        public TypesOfParkingEdit(ContentDialog CallingDialog, Func<Task> UpdateMethod)
        {
            InitializeComponent();
            ParentDialog = CallingDialog;
            this.UpdateMethod = UpdateMethod;
        }
        public TypesOfParkingEdit(ContentDialog CallingDialog, TypesOfParking Instance, Func<Task> UpdateMethod)
        {
            InitializeComponent();
            ParentDialog = CallingDialog;
            this.UpdateMethod = UpdateMethod;
            _isAddition = false;
            ChangingInstance = Instance;
            TypeName.Text = Instance.TypeName;
        }

        private void SaveChanges_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TypeName.Text))
            {
                MessageBox.Show("Значение названия типа не может быть пустым!", "Ошибка сохранения");
                return;
            }
            if (_isAddition)
            {
                TypesOfParking Instance = new TypesOfParking()
                {
                    TypeName = TypeName.Text,
                };
                DbUtils.db.TypesOfParking.Add(Instance);
            }
            else
            {
                ChangingInstance.TypeName = TypeName.Text;
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
