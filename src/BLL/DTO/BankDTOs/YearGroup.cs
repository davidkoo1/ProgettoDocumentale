using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DTO.BankDTOs
{
    public class YearGroup
    {
        public int Year { get; set; }
        public List<MonthGroup> Months { get; set; }
    }

    public class MonthGroup
    {
        public string MonthName { get; set; }
        public List<DocumentTypeGroup> DocumentTypes { get; set; }
    }

    public class DocumentTypeGroup
    {
        public string Name { get; set; }
        public int Count { get; set; }
        public List<Report> Reports { get; set; }
    }
    public class Report
    {
        public string Text { get; set; }
        public int Id { get; set; }
    }
}
