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
    public class InstitutionConfiguration : EntityTypeConfiguration<Institution>
    {
        public InstitutionConfiguration()
        {
            HasKey(institution => institution.Id);


            Property(i => i.InstCode)
                .HasMaxLength(5)
                .IsRequired();

            Property(i => i.Name)
                .HasMaxLength(255)
                .IsRequired();

            Property(a => a.AdditionalInfo)
                .HasMaxLength(1000).IsOptional();

        }
    }
}
