using BLL.DTO;
using BLL.TableParameters;
using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Common.Interfaces
{
    public interface IUserRepository
    {

        Task<IEnumerable<UserDto>> GetAllUsers(DataTablesParameters parameters);
        Task<UserDto> GetUser(int id);
        bool Add(UserDto course);
        bool Update(UserDto course);
        bool Delete(UserDto course);
        bool Save();
    }
}
