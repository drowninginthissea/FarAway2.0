using System;
using System.Collections.Generic;

namespace FarAway2._0.Entities
{
    public partial class ParkingSpaceRental
    {
        public ParkingSpaceRental()
        {
            AdditionalServicesForRent = new HashSet<AdditionalServicesForRent>();
        }

        public int id { get; set; }
        public int idTypeOfRentByDuration { get; set; }
        public int idUser { get; set; }
        public int idParkingSpot { get; set; }
        public DateTime RentalStartDate { get; set; }
        public DateTime? RentEndDate { get; set; }

        public virtual ParkingSpots idParkingSpotNavigation { get; set; }
        public virtual TypeOfRentByDuration idTypeOfRentByDurationNavigation { get; set; }
        public virtual Users idUserNavigation { get; set; }
        public virtual ICollection<AdditionalServicesForRent> AdditionalServicesForRent { get; set; }
    }
}
