using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public class Project
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTill { get; set; }
        public string AdditionalInfo { get; set; } //IsOptional
        public bool IsActive { get; set; }

        public int? InstitutionId { get; set; }
        public virtual Institution Institution { get; set; }

        public int UserId { get; set; }
        public virtual User User { get; set; }

        public virtual List<Document> Documents { get; set; }
    }
}
