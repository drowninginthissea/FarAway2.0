using System;
using System.Collections.Generic;

namespace FarAway2._0.Entities
{
    public partial class BranchCharacteristics
    {
        public int id { get; set; }
        public int CountOfParkingSpaces { get; set; }
        public decimal TheCostOfAParkingSpacePerDay { get; set; }
        public double WidthOfTheLiftingAndLoweringMechanismInMillimeters { get; set; }
        public double TotalWidthOfTheCarInMillimeters { get; set; }
        public double MaximumCarLengthInMillimeters { get; set; }
        public double MaximumHeightOfTheVehicleInMillimeters { get; set; }
        public double MaximumVehicleWeightInKilograms { get; set; }

        public virtual Branches idNavigation { get; set; } = null!;
    }
}
