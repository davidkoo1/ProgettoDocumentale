using BLL.DTO;
using BLL.UserDTOs;
using DAL.Entities;
using System.Threading.Tasks;

namespace BLL.Common.Interfaces
{
    public interface IAccountRepository
    {
        Task<UserDto> GetUserForLogin(LoginDto UserToLogin);
    }
}
