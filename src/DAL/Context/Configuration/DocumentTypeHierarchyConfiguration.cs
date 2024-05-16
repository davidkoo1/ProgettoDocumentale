using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Context.Configuration
{
    public class DocumentTypeHierarchyConfiguration : EntityTypeConfiguration<DocumentTypeHierarchy>
    {
        public DocumentTypeHierarchyConfiguration()
        {
            HasKey(dth => new { dth.IdMacro, dth.IdMicro });

            HasRequired(dth => dth.Macro)
                .WithMany(dt => dt.Macro)
                .HasForeignKey(dth => dth.IdMacro)
                .WillCascadeOnDelete(false);

            HasRequired(dth => dth.Micro)
                .WithMany(dt => dt.Micro)
                .HasForeignKey(dth => dth.IdMicro)
                .WillCascadeOnDelete(false);
        }
    }


}
