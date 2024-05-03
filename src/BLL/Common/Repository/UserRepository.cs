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

        public bool Add(UserDto course)
        {
            throw new NotImplementedException();
        }

        public bool Delete(int userId)
        {
            var userToDelete = _dbContext.Users.Include(x => x.UserRole).FirstOrDefault();
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

        public bool Save() => _dbContext.SaveChanges() > 0 ? true : false;

        public bool Update(UserDto course)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> UserExists(int userId) => await _dbContext.Users.AnyAsync(x => x.Id == userId && !x.IsEnabled);
    }
}
