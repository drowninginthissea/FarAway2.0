using System;
using System.Collections.Generic;

namespace FarAway2._0.Entities
{
    public partial class ListOfAdditionalServices
    {
        public ListOfAdditionalServices()
        {
            AdditionalServicesForRent = new HashSet<AdditionalServicesForRent>();
        }

        public int id { get; set; }
        public string ServiceName { get; set; } = null!;
        public decimal ServicePrice { get; set; }
        public string ServiceDescription { get; set; } = null!;

        public virtual ICollection<AdditionalServicesForRent> AdditionalServicesForRent { get; set; }
    }
}
