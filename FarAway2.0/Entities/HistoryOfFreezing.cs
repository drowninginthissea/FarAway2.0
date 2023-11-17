using System;
using System.Collections.Generic;

namespace FarAway2._0.Entities
{
    public partial class HistoryOfFreezing
    {
        public int idRental { get; set; }
        public int FreezingNumber { get; set; }
        public int idAction { get; set; }
        public DateTime DateOfAction { get; set; }

        public virtual ListOfActions idActionNavigation { get; set; } = null!;
        public virtual ParkingSpaceRental idRentalNavigation { get; set; } = null!;
    }
}
