using System.Collections.Generic;

namespace DAL.Entities
{
    public class Role
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public virtual List<UserRole> UserRoles { get; set; }
    }
}
