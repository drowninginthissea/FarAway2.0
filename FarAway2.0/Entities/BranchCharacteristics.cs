using System;
using System.Collections.Generic;

namespace FarAway2._0.Entities
{
    public partial class BranchCharacteristics
    {
        public int id { get; set; }
        public int CountOfParkingSpaces { get; set; }
        public decimal TheCostOfAParkingSpacePerDay { get; set; }
        public int WidthOfTheLiftingAndLoweringMechanismInMillimeters { get; set; }
        public int TotalWidthOfTheCarInMillimeters { get; set; }
        public int MaximumCarLengthInMillimeters { get; set; }
        public int MaximumHeightOfTheVehicleInMillimeters { get; set; }
        public int MaximumVehicleWeightInKilograms { get; set; }

        public virtual Branches idNavigation { get; set; } = null!;
    }
}
