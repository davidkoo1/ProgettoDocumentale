using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Context.Configuration
{
    public class UserRoleConfiguration : EntityTypeConfiguration<UserRole>
    {
        public UserRoleConfiguration()
        {
            HasKey(ur => ur.UserId);  // Using UserId as the primary key, RoleId becomes unique automatically

            HasRequired(ur => ur.User)
                .WithRequiredDependent(u => u.UserRole);

            HasRequired(ur => ur.Role)
                .WithRequiredDependent(r => r.UserRole);
        }
    }

}
