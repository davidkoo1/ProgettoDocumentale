using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public class Institution
    {
        public int Id { get; set; }
        public string InstCode { get; set; }
        public string Name { get; set; }
        public string AdditionalInfo { get; set; }

        public virtual List<User> Users { get; set; }
        public virtual List<Project> Projects { get; set; }
        public List<Document> Documents { get; set; } //Optional

    }
}
