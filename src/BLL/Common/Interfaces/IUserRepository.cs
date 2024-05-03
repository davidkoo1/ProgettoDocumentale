using BLL.DTO;
using BLL.TableParameters;
using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Common.Interfaces
{
    public interface IUserRepository
    {

        Task<IEnumerable<UserDto>> GetAllUsers(DataTablesParameters parameters);
        Task<UserDto> GetUser(int id);
        Task<bool> UserExists(int userId);
        Task<IEnumerable<RoleDto>> GetRolesAsync();
        bool Add(CreateUserDto userToCreateDto);
        bool Update(UserDto userToUpdate);
        bool Delete(int userId);
        bool Save();
    }
}
