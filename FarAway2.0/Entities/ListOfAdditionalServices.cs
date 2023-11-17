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
        public string SeviceName { get; set; } = null!;
        public decimal SevicePrice { get; set; }
        public string SeviceDescription { get; set; } = null!;

        public virtual ICollection<AdditionalServicesForRent> AdditionalServicesForRent { get; set; }
    }
}
