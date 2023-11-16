using System;
using System.Collections.Generic;

namespace FarAway2._0.Entities
{
    public partial class AdditionalServicesForRent
    {
        public int idRent { get; set; }
        public int idService { get; set; }
        public int FrequencyOfServicePerformanceInDays { get; set; }
        public int idServiceProviders { get; set; }

        public virtual FrequencyOfServices FrequencyOfServicePerformanceInDaysNavigation { get; set; }
        public virtual ParkingSpaceRental idRentNavigation { get; set; }
        public virtual ListOfAdditionalServices idServiceNavigation { get; set; }
        public virtual ServiceProviders idServiceProvidersNavigation { get; set; }
    }
}
