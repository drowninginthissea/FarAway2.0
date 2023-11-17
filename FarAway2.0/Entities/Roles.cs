using System;
using System.Collections.Generic;

namespace FarAway2._0.Entities
{
    public partial class Roles
    {
        public Roles()
        {
            Users = new HashSet<Users>();
        }

        public int id { get; set; }
        public string RoleName { get; set; } = null!;

        public virtual ICollection<Users> Users { get; set; }
    }
}
