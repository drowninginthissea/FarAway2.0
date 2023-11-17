using System;
using System.Collections.Generic;

namespace FarAway2._0.Entities
{
    public partial class TypesOfParking
    {
        public TypesOfParking()
        {
            Branches = new HashSet<Branches>();
        }

        public int id { get; set; }
        public string TypeName { get; set; } = null!;

        public virtual ICollection<Branches> Branches { get; set; }
    }
}
