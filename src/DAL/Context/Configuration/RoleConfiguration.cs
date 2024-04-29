using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Context.Configuration
{
    public class RoleConfiguration : EntityTypeConfiguration<Role>
    {
        public RoleConfiguration()
        {
            HasKey(role => role.Id);

            Property(role => role.Name)
                .HasMaxLength(50)
                .IsRequired();

            HasIndex(role => role.Name)
                .IsUnique();
        }
    }

}
