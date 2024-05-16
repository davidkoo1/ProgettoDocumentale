using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public class DocumentType
    {
        public int Id { get; set; }
        public string Code { get; set; } //Optional
        public string Name { get; set; }
        public string TypeDscr { get; set; } //Optional
        public bool IsMacro { get; set; }
        public bool IsDateGrouped { get; set; }

        public virtual List<Document> Documents { get; set; }

        public virtual ICollection<DocumentTypeHierarchy> Macro { get; set; }
        public virtual ICollection<DocumentTypeHierarchy> Micro { get; set; }
    }

}
