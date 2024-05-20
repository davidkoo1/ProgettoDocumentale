using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DTO.DocumentDTOs
{
    public class DocumentTypeHierarchyDto
    {
        public int IdMacro { get; set; }
        public int IdMicro { get; set; }

        public virtual DocumentTypeDto Macro { get; set; }
        public virtual DocumentTypeDto Micro { get; set; }
    }
}
