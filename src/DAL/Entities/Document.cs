using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public class Document
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string SavedPath { get; set; }
        public DateTime UploadDate { get; set; }
        public string AdditionalInfo { get; set; } //Optional
        public DateTime GroupingDate { get; set; }

        public int? InstitutionId { get; set; }
        public virtual Institution Institution { get; set; }

        public int UserId { get; set; }
        public virtual User User { get; set; }

        public int TypeId { get; set; }
        public virtual DocumentType DocumentType { get; set; }
    }
}
