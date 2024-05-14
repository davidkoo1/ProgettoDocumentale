using AutoMapper;
using BLL.Common.Interfaces;
using BLL.DTO;
using BLL.UserDTOs;
using DAL.Context.Persistance;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Common.Repository
{
    public class AccountRepository : IAccountRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;
        public AccountRepository(ApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<UserDto> GetUserForLogin(LoginDto UserToLogin)
        {
            var user = await _dbContext.Users.Include(u => u.UserRoles.Select(ur => ur.Role)).FirstOrDefaultAsync(u => u.UserName == UserToLogin.Username);
            var encrypted = HashPW(UserToLogin.Password);
            if (user != null && user.Password == encrypted)
            {
                return _mapper.Map<UserDto>(user);
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
