using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DTO.DocumentDTOs
{
    public class DocumentDetailDto
    {
        public string File { get; set; }
        public string Institution { get; set; }
        public string MacroType { get; set; }
        public string MicroType { get; set; }
        public string Project { get; set; }
        public string DataGroup { get; set; }
        public string AdditionalInfo { get; set; }
    }
}
