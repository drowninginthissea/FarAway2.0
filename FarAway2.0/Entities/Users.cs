using System;
using System.Collections.Generic;

namespace FarAway2._0.Entities
{
    public partial class Users
    {
        public Users()
        {
            ParkingSpaceRental = new HashSet<ParkingSpaceRental>();
        }

        public int id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public string PassportSeries { get; set; }
        public string PassportNumber { get; set; }
        public int? idRole { get; set; }

        public virtual Roles idRoleNavigation { get; set; }
        public virtual ICollection<ParkingSpaceRental> ParkingSpaceRental { get; set; }
    }
}
