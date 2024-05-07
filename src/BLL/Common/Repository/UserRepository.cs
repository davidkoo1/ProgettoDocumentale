using AutoMapper;
using BLL.Common.Extensions;
using BLL.Common.Interfaces;
using BLL.DTO;
using BLL.TableParameters;
using DAL.Context.Persistance;
using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Common.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _dbContext;

        private readonly IMapper _mapper;

        public UserRepository(ApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public bool Add(CreateUserDto userToCreateDto)
        {
            var existing = _dbContext.Users.Any(x => (x.UserName == userToCreateDto.UserName || x.Email == userToCreateDto.Email));

            if (userToCreateDto == null || existing == true)
            {
                return false;
            }
            var userToCreate = _mapper.Map<User>(userToCreateDto);
            string defaultPW = "Cedacri1234567!";
            userToCreate.Password = HashPW(defaultPW);
            userToCreate.UserRole = new UserRole
            {
                RoleId = userToCreateDto.RoleId
            };
            _dbContext.Users.AddOrUpdate(userToCreate);
            return Save();
        }

        public bool Delete(int userId)
        {
            var userToDelete = _dbContext.Users.Include(x => x.UserRole).FirstOrDefault(x => x.Id == userId);
            userToDelete.IsEnabled = false;
            _dbContext.Users.AddOrUpdate(userToDelete);
            return Save();
        }

        public async Task<IEnumerable<UserDto>> GetAllUsers(DataTablesParameters parameters) => _mapper.Map<IEnumerable<UserDto>>
            (await _dbContext.Users
            .Search(parameters)
            .OrderBy(parameters)
            .Page(parameters)
            .ToListAsync());

        public async Task<IEnumerable<RoleDto>> GetRolesAsync() => _mapper.Map<IEnumerable<RoleDto>>(await _dbContext.Roles.ToListAsync());

        public async Task<UpdateUserDto> GetUpdateUser(int id) => _mapper.Map<UpdateUserDto>(await _dbContext.Users.Include(x => x.UserRole).FirstOrDefaultAsync(x => x.Id == id));

        public async Task<UserDto> GetUser(int id)
        {
            // Include UserRole and Role in the initial query to reduce DB round trips
            var user = await _dbContext.Users
                .Include(u => u.UserRole)
                .FirstOrDefaultAsync(x => x.Id == id);
            var roles = await _dbContext.UserRoles.Include(x => x.Role).FirstOrDefaultAsync(x => x.UserId == id);
            var role = await _dbContext.Roles.FirstOrDefaultAsync(x => x.Id == roles.RoleId);
            return new UserDto
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                IsEnabled = user.IsEnabled,
                Name = user.Name,
                Surname = user.Surname,
                Patronymic = user.Patronymic,
                UserRole = role.Name

            };
        }

        public bool Save() => _dbContext.SaveChanges() > 0 ? true : false;

        public bool Update(UpdateUserDto userToCreateDto)
        {
            var user = _dbContext.Users.Include(x => x.UserRole).FirstOrDefault(x => x.Id == userToCreateDto.Id);
            user.Email = userToCreateDto.Email;
            user.Name = userToCreateDto.Name;
            user.Surname = userToCreateDto.Surname;
            user.Patronymic = userToCreateDto.Patronymic;
            user.IsEnabled = userToCreateDto.IsEnabled;
            // Удаляем старые роли пользователя
            var userRolesToRemove = _dbContext.UserRoles.Where(ur => ur.UserId == userToCreateDto.Id);
            _dbContext.UserRoles.RemoveRange(userRolesToRemove);

            user.UserRole = new UserRole
            {
                RoleId = userToCreateDto.RoleId,
            };
            _dbContext.Users.AddOrUpdate(user);
            return Save();
        }

        public async Task<bool> UserExists(int userId) => await _dbContext.Users.AnyAsync(x => x.Id == userId && !x.IsEnabled);

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
