using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DTO.DocumentDTOs
{
    public class DocumentDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string SavedPath { get; set; }
        public DateTime UploadDate { get; set; }
        public string AdditionalInfo { get; set; } //Optional
        public DateTime GroupingDate { get; set; }

        public string InstitutionId { get; set; }
        public string TypeId { get; set; }
    }
}
