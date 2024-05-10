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

        public async Task<bool> Add(CreateUserDto userToCreateDto)
        {
            var existing = await _dbContext.Users.AnyAsync(x => (x.UserName == userToCreateDto.UserName || x.Email == userToCreateDto.Email));

            if (userToCreateDto == null || existing == true)
            {
                return false;
            }
            var userToCreate = _mapper.Map<User>(userToCreateDto);
            string defaultPW = "Cedacri1234567!";
            userToCreate.Password = HashPW(defaultPW);
            userToCreate.UserRoles = userToCreateDto.RolesId.Select(id => new UserRole { RoleId = id }).ToList();
            _dbContext.Users.Add(userToCreate);
            return await Save();
        }

        public async Task<bool> Delete(int userId)
        {
            var userToDelete = _dbContext.Users.FirstOrDefault(x => x.Id == userId);
            userToDelete.IsEnabled = false;
            _dbContext.Users.AddOrUpdate(userToDelete);
            return await Save();
        }

        public async Task<IEnumerable<UserDto>> GetAllUsers(DataTablesParameters parameters) => _mapper.Map<IEnumerable<UserDto>>
            (await _dbContext.Users
            .Search(parameters)
            .OrderBy(parameters)
            .Page(parameters)
            .ToListAsync());

        public async Task<IEnumerable<RoleDto>> GetRolesAsync() => _mapper.Map<IEnumerable<RoleDto>>(await _dbContext.Roles.ToListAsync());

        public async Task<UpdateUserDto> GetUpdateUser(int id) => _mapper.Map<UpdateUserDto>(await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == id));

        public async Task<UserDto> GetUser(int id)
        {
            var user = await _dbContext.Users.Include(u => u.UserRoles.Select(ur => ur.Role)).FirstOrDefaultAsync(u => u.Id == id);

            return _mapper.Map<UserDto>(user);
        }

        public async Task<bool> Save() => await _dbContext.SaveChangesAsync() > 0 ? true : false;

        public async Task<bool> Update(UpdateUserDto userToUpdateDto)
        {
            var user = _dbContext.Users.FirstOrDefault(x => x.Id == userToUpdateDto.Id);
            if (user == null)
                return false;
            var userToUpdate = _mapper.Map<User>(userToUpdateDto);

            userToUpdate.UserName = user.UserName;
            userToUpdate.Password = user.Password;

            // Обновляем роли пользователя
            var existingRoles = _dbContext.UserRoles.Where(ur => ur.UserId == user.Id).ToList();
            _dbContext.UserRoles.RemoveRange(existingRoles); // Удаление старых ролей

            // Добавляем новые роли
            var newRoles = userToUpdateDto.RolesId.Select(id => new UserRole { UserId = user.Id, RoleId = id }).ToList();
            _dbContext.UserRoles.AddRange(newRoles);

            _dbContext.Users.AddOrUpdate(userToUpdate);
            return await Save();
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
