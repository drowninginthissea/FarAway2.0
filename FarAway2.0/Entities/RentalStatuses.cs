using System;
using System.Collections.Generic;

namespace FarAway2._0.Entities
{
    public partial class RentalStatuses
    {
        public RentalStatuses()
        {
            ParkingSpaceRental = new HashSet<ParkingSpaceRental>();
        }

        public int id { get; set; }
        public string StatusName { get; set; } = null!;

        public virtual ICollection<ParkingSpaceRental> ParkingSpaceRental { get; set; }
    }
}
