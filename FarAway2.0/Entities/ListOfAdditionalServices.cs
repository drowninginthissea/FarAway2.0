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
        public string SeviceName { get; set; }
        public decimal SevicePrice { get; set; }
        public string SeviceDescription { get; set; }

        public virtual ICollection<AdditionalServicesForRent> AdditionalServicesForRent { get; set; }
    }
}
