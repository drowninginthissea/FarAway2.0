using Microsoft.EntityFrameworkCore;
using ModernWpf.Controls;
using System.Windows.Controls;

namespace FarAway2._0.Content.Controls.UserControls.JournalTables.Branches
{
    public partial class BranchesEdit : UserControl, IContentDialogParent
    {
        public ContentDialog ParentDialog { get; set; }
        private bool _isAddition = true;
        Entities.Branches ChangingInstance;
        Func<Task> UpdateMethod;
        public BranchesEdit(ContentDialog CallingDialog, Func<Task> UpdateMethod)
        {
            InitializeComponent();
            ParentDialog = CallingDialog;
            this.UpdateMethod = UpdateMethod;
            FillComboBoxes();
        }
        public BranchesEdit(ContentDialog CallingDialog, Func<Task> UpdateMethod, Entities.Branches Branch)
        {
            InitializeComponent();
            ParentDialog = CallingDialog;
            this.UpdateMethod = UpdateMethod;
            _isAddition = false;
            ChangingInstance = Branch;
            FillComboBoxes();
            AddressTB.Text = Branch.Address;
            TypeOfParkingCB.SelectedItem = Branch.idTypeOfParkingNavigation;
            TypeOfCarExchangeSystemCB.SelectedItem = Branch.idTypeOfCarExchangeSystemNavigation;
            CountOfParkingSpacesTB.Text = Branch.BranchCharacteristics.CountOfParkingSpaces.ToString();
            TheCostOfAParkingSpacePerDayTB.Text = $"{Branch.BranchCharacteristics.TheCostOfAParkingSpacePerDay:0.00}";
            WidthOfTheLiftingAndLoweringMechanismInMillimetersTB.Text = 
                Branch.BranchCharacteristics.WidthOfTheLiftingAndLoweringMechanismInMillimeters.ToString();
            TotalWidthOfTheCarInMillimetersTB.Text = Branch.BranchCharacteristics.TotalWidthOfTheCarInMillimeters.ToString() ;
            MaximumCarLengthInMillimetersTB.Text = Branch.BranchCharacteristics.MaximumCarLengthInMillimeters.ToString();
            MaximumHeightOfTheVehicleInMillimetersTB.Text =
                Branch.BranchCharacteristics.MaximumHeightOfTheVehicleInMillimeters.ToString();
            MaximumVehicleWeightInKilogramsTB.Text = Branch.BranchCharacteristics.MaximumVehicleWeightInKilograms.ToString();
        }
        private async void FillComboBoxes()
        {
            TypeOfParkingCB.ItemsSource = await DbUtils.db.TypesOfParking.ToListAsync();
            TypeOfCarExchangeSystemCB.ItemsSource = await DbUtils.db.TypesOfCarExchangeSystem.ToListAsync();
        }
        private void SaveChanges_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(AddressTB.Text))
            {
                MessageBox.Show("Значение адреса не должно быть пустым!", "Ошибка сохранения");
                return;
            }
            if (TypeOfParkingCB.SelectedIndex == -1)
            {
                MessageBox.Show("Значение типа парковки должно быть выбрано!", "Ошибка сохранения");
                return;
            }
            if (TypeOfCarExchangeSystemCB.SelectedIndex == -1)
            {
                MessageBox.Show("Значение типа обмена автомобилями должно быть выбрано!", "Ошибка сохранения");
                return;
            }
            if (!int.TryParse(CountOfParkingSpacesTB.Text, out int CountOfSpaces) ||
                CountOfSpaces < 0)
            {
                MessageBox.Show("Значение количества парковочных мест введено неверно!", "Ошибка сохранения");
                return;
            }
            if (!decimal.TryParse(TheCostOfAParkingSpacePerDayTB.Text, out decimal CostPerDay) ||
                !DbUtils.ValidateDecimal(CostPerDay, 10, 2) ||
                CostPerDay < 0)
            {
                MessageBox.Show("Значение стоимости аренды в день введено не корректно!", "Ошибка сохранения");
                return;
            }
            if (!int.TryParse(WidthOfTheLiftingAndLoweringMechanismInMillimetersTB.Text, out int WidthOfMechanism) ||
                WidthOfMechanism < 0)
            {
                MessageBox.Show("Значение ширины подъёмно-опускающего механизма введено не корректно!", "Ошибка сохранения");
                return;
            }
            if (!int.TryParse(TotalWidthOfTheCarInMillimetersTB.Text, out int WidthOfCar) ||
                WidthOfCar < 0)
            {
                MessageBox.Show("Значение максимальной ширины авто введено не корректно!", "Ошибка сохранения");
                return;
            }
            if (!int.TryParse(MaximumCarLengthInMillimetersTB.Text, out int LengthOfCar) ||
                LengthOfCar < 0)
            {
                MessageBox.Show("Значение максимальной длины авто введено не корректно!", "Ошибка сохранения");
                return;
            }
            if (!int.TryParse(MaximumHeightOfTheVehicleInMillimetersTB.Text, out int HeightOfCar) ||
                HeightOfCar < 0)
            {
                MessageBox.Show("Значение максимальной высоты авто введено не корректно!", "Ошибка сохранения");
                return;
            }
            if (!int.TryParse(MaximumVehicleWeightInKilogramsTB.Text, out int WeightOfCar) ||
                WeightOfCar < 0)
            {
                MessageBox.Show("Значение максимального веса авто введено не корректно!", "Ошибка сохранения");
                return;
            }
            if (_isAddition)
            {
                Entities.Branches BranchInstance = new Entities.Branches()
                {
                    idTypeOfParking = (TypeOfParkingCB.SelectedItem as TypesOfParking).id,
                    idTypeOfCarExchangeSystem = (TypeOfCarExchangeSystemCB.SelectedItem as TypesOfCarExchangeSystem).id,
                    Address = AddressTB.Text
                };
                BranchCharacteristics Characteristics = new BranchCharacteristics()
                {
                    CountOfParkingSpaces = CountOfSpaces,
                    TheCostOfAParkingSpacePerDay = CostPerDay,
                    WidthOfTheLiftingAndLoweringMechanismInMillimeters = WidthOfMechanism,
                    TotalWidthOfTheCarInMillimeters = WidthOfCar,
                    MaximumCarLengthInMillimeters = LengthOfCar,
                    MaximumHeightOfTheVehicleInMillimeters = HeightOfCar,
                    MaximumVehicleWeightInKilograms = WeightOfCar
                };
                Characteristics.idNavigation = BranchInstance;
                DbUtils.db.Branches.Add(BranchInstance);
                DbUtils.db.BranchCharacteristics.Add(Characteristics);
            }
            else
            {
                ChangingInstance.idTypeOfParking = (TypeOfParkingCB.SelectedItem as TypesOfParking).id;
                ChangingInstance.idTypeOfCarExchangeSystem = (TypeOfCarExchangeSystemCB.SelectedItem as TypesOfCarExchangeSystem).id;
                ChangingInstance.Address = AddressTB.Text;
                ChangingInstance.BranchCharacteristics.CountOfParkingSpaces = CountOfSpaces;
                ChangingInstance.BranchCharacteristics.TheCostOfAParkingSpacePerDay = CostPerDay;
                ChangingInstance.BranchCharacteristics.WidthOfTheLiftingAndLoweringMechanismInMillimeters = WidthOfMechanism;
                ChangingInstance.BranchCharacteristics.TotalWidthOfTheCarInMillimeters = WidthOfCar;
                ChangingInstance.BranchCharacteristics.MaximumCarLengthInMillimeters = LengthOfCar;
                ChangingInstance.BranchCharacteristics.MaximumHeightOfTheVehicleInMillimeters = HeightOfCar;
                ChangingInstance.BranchCharacteristics.MaximumVehicleWeightInKilograms = WeightOfCar;
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
