using System;
using System.Collections.Generic;

namespace FarAway2._0.Entities
{
    public partial class ParkingSpots
    {
        public ParkingSpots()
        {
            ParkingSpaceRental = new HashSet<ParkingSpaceRental>();
        }

        public int id { get; set; }
        public int idBranch { get; set; }
        public int idParkingSpotStatus { get; set; }

        public virtual Branches idBranchNavigation { get; set; } = null!;
        public virtual ParkingSpotStatuses idParkingSpotStatusNavigation { get; set; } = null!;
        public virtual ICollection<ParkingSpaceRental> ParkingSpaceRental { get; set; }
    }
}
