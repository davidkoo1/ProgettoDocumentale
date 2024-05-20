using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DTO.DocumentDTOs
{
    public class DocumentTypeDto
    {
        public int Id { get; set; }
        public string Code { get; set; } //Optional
        public string Name { get; set; }
        public string TypeDscr { get; set; } //Optional
        public bool IsMacro { get; set; }
        public bool IsDateGrouped { get; set; }

        public virtual List<DocumentDto> Documents { get; set; }

        public ICollection<DocumentTypeHierarchyDto> Macro { get; set; }
        public ICollection<DocumentTypeHierarchyDto> Micro { get; set; }
    }
}
