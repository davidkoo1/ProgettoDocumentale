using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DTO.BankDTOs
{
    public class ServiceGroupTreeVm
    {
        public int Year { get; set; }
        public List<MonthServiceGroup> Months { get; set; }
    }

    public class MonthServiceGroup
    {
        public string MonthName { get; set; }
        public List<DocumentTypeGroup> DocumentTypes { get; set; }
    }

    public class DocumentTypeGroup
    {
        public string Name { get; set; }
        public int Count { get; set; }
        public List<ReportVm> Reports { get; set; }
    }
}
