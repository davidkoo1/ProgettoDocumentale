using DAL.Entities;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;

namespace DAL.Context
{
    public class DbInitializer : DropCreateDatabaseIfModelChanges<ApplicationDbContext>
    {
        protected override void Seed(ApplicationDbContext context)
        {
            //// Создание ролей
            //var roles = new List<Role>
            //{
            //    new Role { Id = 1, Name = "Admin" },
            //    new Role { Id = 2, Name = "Operator" },
            //    new Role { Id = 3, Name = "User" }
            //};
            //roles.ForEach(r => context.Roles.AddOrUpdate(new Role { Id = r.Id, Name = r.Name }));

            // Создание пользователя
            var user = new User
            {
                Id = 1,
                UserName = "Cr00001",
                Password = "8c6976e5b5410415bde908bd4dee15dfb167a9c873fc4bb8a81f6f2ab448a918", // Хешированный пароль
                Name = "Admin",
                Surname = "Admin",
                Patronymic = "Admin",
                Email = "admin@mail.com",
                IsEnabled = true

            };
            //users.ForEach(u => context.Users.AddOrUpdate(u => u.Id, u));
            context.Users.AddOrUpdate(user);

            // Создание связей пользователь-роль
            //var userRoles = new List<UserRole>
            //{
            //    new UserRole { UserId = 1, RoleId = 1 } // Связь пользователя Admin с ролью Admin
            //};
            ////userRoles.ForEach(ur => context.UserRoles.AddOrUpdate(ur => new { ur.UserId, ur.RoleId }, ur));
            //context.UserRoles.AddRange(userRoles);
            // Сохранение изменений
            context.SaveChanges();

            base.Seed(context);
        }
    }

}
