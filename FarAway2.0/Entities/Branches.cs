using System;
using System.Collections.Generic;

namespace FarAway2._0.Entities
{
    public partial class Branches
    {
        public Branches()
        {
            ParkingSpots = new HashSet<ParkingSpots>();
        }

        public int id { get; set; }
        public int idTypeOfParking { get; set; }
        public int? idTypeOfCarExchangeSystem { get; set; }
        public string Address { get; set; }

        public virtual TypesOfCarExchangeSystem idTypeOfCarExchangeSystemNavigation { get; set; }
        public virtual TypesOfParking idTypeOfParkingNavigation { get; set; }
        public virtual BranchCharacteristics BranchCharacteristics { get; set; }
        public virtual ICollection<ParkingSpots> ParkingSpots { get; set; }
    }
}
