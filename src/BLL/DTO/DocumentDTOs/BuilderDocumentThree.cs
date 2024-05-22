using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DTO.DocumentDTOs
{
    public class BuilderDocumentThree
    {
        public int InstitutionId { get; set; }
        public string InstitutionName { get; set; }
        public List<YearDocumentTypeGroup> YearGroups { get; set; }
    }

    public class YearDocumentTypeGroup
    {
        public int Year { get; set; }
        public List<int> SubTypeIds { get; set; }
        public List<string> SubTypeNames { get; set; }
    }
}
