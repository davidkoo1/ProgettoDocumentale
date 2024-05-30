using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DTO.BankDTOs
{

    public class SLAGroupTreeVm
    {
        public int Year { get; set; }
        public List<MonthSLAGroup> Months { get; set; }
    }

    public class MonthSLAGroup
    {
        public string MonthName { get; set; }
        public int Count { get; set; }
        public List<ReportVm> Reports { get; set; }
    }

}
