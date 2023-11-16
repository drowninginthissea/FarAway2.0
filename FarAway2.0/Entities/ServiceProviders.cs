using System;
using System.Collections.Generic;

namespace FarAway2._0.Entities
{
    public partial class ServiceProviders
    {
        public ServiceProviders()
        {
            AdditionalServicesForRent = new HashSet<AdditionalServicesForRent>();
        }

        public int id { get; set; }
        public string Name { get; set; }
        public string ITIN { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }

        public virtual ICollection<AdditionalServicesForRent> AdditionalServicesForRent { get; set; }
    }
}
