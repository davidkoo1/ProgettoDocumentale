using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DTO.InstitutionDTOs
{
    public class InstitutionDto
    {
        public int Id { get; set; }
        public string InstCode { get; set; }
        public string Name { get; set; }
        public string AdditionalInfo { get; set; }

        //public virtual List<User> Users { get; set; }
    }
}
