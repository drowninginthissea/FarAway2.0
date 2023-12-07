using ModernWpf.Controls;
using System.Windows.Controls;

namespace FarAway2._0.Content.Controls.UserControls.ReferenceTables.DataEdit
{
    public partial class TypeOfRentByDurationEdit : UserControl, IContentDialogParent
    {
        public ContentDialog ParentDialog { get; set; }
        private bool _isAddition = true;
        TypeOfRentByDuration ChangingInstance;
        Func<Task> UpdateMethod;
        public TypeOfRentByDurationEdit(ContentDialog CallingDialog, Func<Task> UpdateMethod)
        {
            InitializeComponent();
            ParentDialog = CallingDialog;
            this.UpdateMethod = UpdateMethod;
        }
        public TypeOfRentByDurationEdit(ContentDialog CallingDialog, TypeOfRentByDuration Instance, Func<Task> UpdateMethod)
        {
            InitializeComponent();
            ParentDialog = CallingDialog;
            this.UpdateMethod = UpdateMethod;
            _isAddition = false;
            ChangingInstance = Instance;
            TypeName.Text = Instance.TypeName;
            MinDurationOfRentalDays.Text = Instance.MinDurationOfRentalDays.ToString();
            MaxDurationOfRentalDays.Text = Instance?.MaxDurationOfRentalDays?.ToString();
            PriceCoefficient.Text = $"{Instance.PriceCoefficient:0.00}";
        }

        private void SaveChanges_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TypeName.Text))
            {
                MessageBox.Show("Значение названия типа не может быть пустым!", "Ошибка сохранения");
                return;
            }
            if (TypeName.Text.Length > 40)
            {
                MessageBox.Show("Длина названия типа не может превышать 40 символов!", "Ошибка сохранения");
                return;
            }
            if (!int.TryParse(MinDurationOfRentalDays.Text, out _))
            {
                MessageBox.Show("Значение минимального срока аренды введено не корректно!", "Ошибка сохранения");
                return;
            }
            if (MaxDurationOfRentalDays.Text != string.Empty &&
                !int.TryParse(MaxDurationOfRentalDays.Text, out _))
            {
                MessageBox.Show("Значение максимального срока аренды введено не корректно!", "Ошибка сохранения");
                return;
            }
            if (!decimal.TryParse(PriceCoefficient.Text, out decimal ParsedNumber)
                || !DbUtils.ValidateDecimal(ParsedNumber, 3, 2))
            {
                MessageBox.Show("Значение коэффициента цены аренды аренды введено не корректно! (Целая часть может в себе содержать 1 цифру, а дробная 2 цифры)", "Ошибка сохранения");
                return;
            }
            int MinDur = int.Parse(MinDurationOfRentalDays.Text);
            Nullable<int> MaxDur = 
                MaxDurationOfRentalDays.Text == string.Empty ? null : int.Parse(MaxDurationOfRentalDays.Text);
            if (MaxDur.HasValue && (MinDur > MaxDur))
            {
                MessageBox.Show("Значение минимального срока аренды не может превышать срок максимальной " +
                    "длительности аренды!", "Ошибка сохранения");
                return;
            }

            if (_isAddition)
            {
                TypeOfRentByDuration Instance = new TypeOfRentByDuration()
                {
                    TypeName = TypeName.Text,
                    MinDurationOfRentalDays = MinDur,
                    MaxDurationOfRentalDays = MaxDur,
                    PriceCoefficient = decimal.Parse(PriceCoefficient.Text)
                };
                DbUtils.db.TypeOfRentByDuration.Add(Instance);
            }
            else
            {
                ChangingInstance.TypeName = TypeName.Text;
                ChangingInstance.MinDurationOfRentalDays = MinDur;
                ChangingInstance.MaxDurationOfRentalDays = MaxDur;
                ChangingInstance.PriceCoefficient = decimal.Parse(PriceCoefficient.Text);
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
