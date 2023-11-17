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
        public string Surname { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string? Patronymic { get; set; }
        public string Email { get; set; } = null!;
        public string Login { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public byte[]? Photo { get; set; }
        public int idRole { get; set; }

        public virtual Roles idRoleNavigation { get; set; } = null!;
        public virtual ICollection<ParkingSpaceRental> ParkingSpaceRental { get; set; }
    }
}
