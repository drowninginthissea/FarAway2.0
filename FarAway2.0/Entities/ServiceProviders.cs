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
        public string Name { get; set; } = null!;
        public string ITIN { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string Email { get; set; } = null!;

        public virtual ICollection<AdditionalServicesForRent> AdditionalServicesForRent { get; set; }
    }
}
