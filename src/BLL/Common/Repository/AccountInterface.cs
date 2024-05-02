using BLL.Common.Interfaces;
using BLL.DTO;
using DAL.Context.Persistance;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Common.Repository
{
    public class AccountInterface : IAccountInterface
    {
        private readonly ApplicationDbContext _dbContext;
        public AccountInterface(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<UserDto> GetUserForLogin(LoginDto UserToLogin)
        {
            var user = await _dbContext.Users.Include(x => x.UserRole).FirstOrDefaultAsync(u => u.UserName == UserToLogin.Username);

            var encrypted = HashPW(UserToLogin.Password);
            if (user != null && user.Password == encrypted)
            {
                return new UserDto
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    IsEnabled = user.IsEnabled,
                    Name = user.Name,
                    Surname = user.Surname,
                    Patronymic = user.Patronymic,
                    Role = new RoleDto
                    {
                        Id = user.UserRole.RoleId,
                        Name = user.UserRole.Role.Name
                    },
                };
            }
            return null;
        }

        private string HashPW(string password)
        {

            using (SHA256 hash = SHA256.Create())
            {
                return string.Concat(hash
                    .ComputeHash(Encoding.UTF8.GetBytes(password))
                    .Select(item => item.ToString("x2")));
            }
        }
    }
}
