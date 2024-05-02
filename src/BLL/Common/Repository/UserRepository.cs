using BLL.Common.Extensions;
using BLL.Common.Interfaces;
using BLL.DTO;
using BLL.TableParameters;
using DAL.Context.Persistance;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace BLL.Common.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public UserRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public bool Add(UserDto course)
        {
            throw new NotImplementedException();
        }

        public bool Delete(UserDto course)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<UserDto>> GetAllUsers(DataTablesParameters parameters)
        {
            var users = await _dbContext.Users
            .Search(parameters)
            .OrderBy(parameters)
            .Page(parameters)
            .ToListAsync();

            var userDtos = users.Select(u => new UserDto
            {
                Id = u.Id,
                UserName = u.UserName,
                Email = u.Email,
                IsEnabled = u.IsEnabled,
                Name = u.Name,
                Surname = u.Surname,
                Patronymic = u.Patronymic,

            }).ToList();

            return userDtos;
        }


        public async Task<UserDto> GetUser(int id) => await _dbContext.Users.Select(u => new UserDto
        {
            Id = u.Id,
            UserName = u.UserName,
            Email = u.Email,
            IsEnabled = u.IsEnabled,
            Name = u.Name,
            Surname = u.Surname,
            Patronymic = u.Patronymic,
            Role = new RoleDto
            {
                Id = u.UserRole.RoleId,
                Name = u.UserRole.Role.Name
            }
        }).FirstOrDefaultAsync(u => u.Id == id);

        public bool Save()
        {
            throw new NotImplementedException();
        }

        public bool Update(UserDto course)
        {
            throw new NotImplementedException();
        }
    }
}
