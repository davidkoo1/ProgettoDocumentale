using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DTO.BankDTOs
{
    public class ProjectGroupTreeVm
    {
        public int Year { get; set; }
        public List<ProjectGroup> ProjectGroups { get; set; }
    }

    public class ProjectGroup
    {
        public string ProjectName { get; set; }
        public List<ProjectTypeGroup> ProjectTypes { get; set; }
    }

    public class ProjectTypeGroup
    {
        public string Name { get; set; }
        public int Count { get; set; }
        public List<ReportVm> Reports { get; set; }
    }
}
