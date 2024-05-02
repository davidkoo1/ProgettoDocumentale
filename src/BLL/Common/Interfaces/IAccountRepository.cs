using BLL.DTO;
using DAL.Entities;
using System.Threading.Tasks;

namespace BLL.Common.Interfaces
{
    public interface IAccountRepository
    {
        Task<UserDto> GetUserForLogin(LoginDto UserToLogin);
    }
}
