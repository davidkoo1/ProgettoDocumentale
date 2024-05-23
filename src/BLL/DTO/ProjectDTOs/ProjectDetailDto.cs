using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DTO.ProjectDTOs
{
    public class ProjectDetailDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string InstitutionName { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTill { get; set; }
        public string AdditionalInfo { get; set; }
        public bool IsActive { get; set; }
    }
}
