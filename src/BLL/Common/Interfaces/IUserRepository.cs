using BLL.UserDTOs;
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
        Task<UpdateUserDto> GetUpdateUser(int id);
        Task<bool> UserExists(int userId);
        Task<IEnumerable<RoleDto>> GetRolesAsync();

        Task<bool> IsEmailInUseAsync(string email); //OnlyForFluentValidation
        Task<bool> IsUserNameInUseAsync(string username); //OnlyForFluentValidation

        Task<bool> Add(CreateUserDto userToCreateDto);
        Task<bool> Update(UpdateUserDto userToUpdateDto);
        Task<bool> Delete(int userId);
        Task<bool> Save();
    }
}
