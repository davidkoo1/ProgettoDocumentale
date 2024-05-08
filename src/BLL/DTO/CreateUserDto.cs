using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DTO
{
    public class CreateUserDto
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public bool IsEnabled { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Patronymic { get; set; }
        public List<int> RolesId { get; set; }

    }
}
