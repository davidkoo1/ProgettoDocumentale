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
    public class DocumentConfiguration : EntityTypeConfiguration<Document>
    {
        public DocumentConfiguration()
        {
            HasKey(document => document.Id);

            Property(d => d.Name)
                .HasMaxLength(255)
                .IsRequired();

            Property(a => a.AdditionalInfo)
                .HasMaxLength(1000).IsOptional();


            HasOptional(i => i.Institution)
               .WithMany(d => d.Documents)
               .HasForeignKey(i => i.InstitutionId);

            HasRequired(i => i.DocumentType)
                .WithMany(d => d.Documents)
                .HasForeignKey(i => i.TypeId);

            HasRequired(user => user.User)
               .WithMany(d => d.Documents)
               .HasForeignKey(user => user.UserId);
        }
    }
}
