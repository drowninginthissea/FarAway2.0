using System;
using System.Collections.Generic;

namespace FarAway2._0.Entities
{
    public partial class ParkingSpotStatuses
    {
        public ParkingSpotStatuses()
        {
            ParkingSpots = new HashSet<ParkingSpots>();
        }

        public int id { get; set; }
        public string StatusName { get; set; }

        public virtual ICollection<ParkingSpots> ParkingSpots { get; set; }
    }
}
