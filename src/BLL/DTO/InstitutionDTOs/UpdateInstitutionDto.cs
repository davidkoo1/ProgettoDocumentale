using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DTO.InstitutionDTOs
{
    public class UpdateInstitutionDto
    {
        public int Id { get; set; }
        public string InstCode { get; set; }
        public string Name { get; set; }
        public string AdditionalInfo { get; set; }
    }
}
