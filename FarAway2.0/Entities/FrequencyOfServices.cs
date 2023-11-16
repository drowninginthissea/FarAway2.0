using System;
using System.Collections.Generic;

namespace FarAway2._0.Entities
{
    public partial class FrequencyOfServices
    {
        public FrequencyOfServices()
        {
            AdditionalServicesForRent = new HashSet<AdditionalServicesForRent>();
        }

        public int id { get; set; }
        public string FrequencyName { get; set; }

        public virtual ICollection<AdditionalServicesForRent> AdditionalServicesForRent { get; set; }
    }
}
