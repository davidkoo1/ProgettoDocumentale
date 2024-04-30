using BLL.DTO;
using DAL.Entities;
using System.Threading.Tasks;

namespace BLL.Common.Interfaces
{
    public interface IAccountInterface
    {
        Task<UserDto> GetUserForLogin(LoginDto UserToLogin);
    }
}
