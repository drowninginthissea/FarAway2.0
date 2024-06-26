﻿using System;
using System.Collections.Generic;

namespace FarAway2._0.Entities
{
    public partial class TypeOfRentByDuration
    {
        public TypeOfRentByDuration()
        {
            ParkingSpaceRental = new HashSet<ParkingSpaceRental>();
        }

        public int id { get; set; }
        public string TypeName { get; set; } = null!;
        public int MinDurationOfRentalDays { get; set; }
        public int? MaxDurationOfRentalDays { get; set; }
        public decimal PriceCoefficient { get; set; }

        public virtual ICollection<ParkingSpaceRental> ParkingSpaceRental { get; set; }
    }
}
