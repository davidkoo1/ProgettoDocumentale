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
    public class DocumentTypeConfiguration : EntityTypeConfiguration<DocumentType>
    {
        public DocumentTypeConfiguration()
        {
            HasKey(dt => dt.Id);

            Property(dt => dt.Code)
                .HasMaxLength(50)
                .IsOptional();

            Property(dt => dt.Name)
                .HasMaxLength(255)
                .IsRequired();

            Property(dt => dt.TypeDscr)
                .HasMaxLength(1000)
                .IsOptional();

        }
    }

}
