using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DTO.DocumentDTOs
{
    public class DocumentViewModel
    {
        public string Institution { get; set; }
        public int Year { get; set; }
        public List<DocumentItem> Documents { get; set; }
    }

    public class DocumentItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public DateTime Date { get; set; }
        public string Institution { get; set; }
       // public string Project { get; set; }
    }

}
