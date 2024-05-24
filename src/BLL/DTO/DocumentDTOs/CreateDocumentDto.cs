using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace BLL.DTO.DocumentDTOs
{
    public class CreateDocumentDto
    {
        public HttpPostedFileBase File { get; set; }
        public string AdditionalInfo { get; set; }
        public DateTime GroupingDate { get; set; }
        public int InstitutionId { get; set; }
        public int MacroId { get; set; }
        public int? MicroId { get; set; }
        public int? ProjectId { get; set; }
    }

}
