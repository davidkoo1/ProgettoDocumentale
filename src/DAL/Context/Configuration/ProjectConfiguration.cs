using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Context.Configuration
{
    public class ProjectConfiguration : EntityTypeConfiguration<Project>
    {
        public ProjectConfiguration()
        {
            HasKey(p => p.Id);

            Property(i => i.Name)
                .HasMaxLength(255)
                .IsRequired();

            Property(a => a.AdditionalInfo)
                .HasMaxLength(1000).IsOptional();

            HasOptional(i => i.Institution)
               .WithMany(d => d.Projects)
               .HasForeignKey(i => i.InstitutionId);


            HasRequired(user => user.User)
               .WithMany(d => d.Projects)
               .HasForeignKey(user => user.UserId);

        }
    }
}
