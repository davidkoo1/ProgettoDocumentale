using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DAL.Entities
{
    public class DocumentTypeHierarchy
    {
        public int IdMacro { get; set; }
        public int IdMicro { get; set; }

        public virtual DocumentType Macro { get; set; }
        public virtual DocumentType Micro { get; set; }
    }
}
