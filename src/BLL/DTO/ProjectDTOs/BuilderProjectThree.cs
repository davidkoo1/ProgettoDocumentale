using BLL.DTO.DocumentDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DTO.ProjectDTOs
{
    public class BuilderProjectThree
    {
        public int InstitutionId { get; set; }
        public string InstitutionName { get; set; }
        public List<int> YearGroups { get; set; }

    }
}
