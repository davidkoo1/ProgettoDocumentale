using DAL.Entities;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Runtime.Remoting.Contexts;

namespace DAL.Context.Persistance
{
    public class DbInitializer : DbMigrationsConfiguration<ApplicationDbContext>
    {
        #region Roles
        public static readonly Role AdminRole = new Role { Id = 1, Name = "Administrator" };
        public static readonly Role OperatorCedacriRole = new Role { Id = 2, Name = "Operator Cedacri" };
        public static readonly Role UserBancarRole = new Role { Id = 3, Name = "Operator Bancar" };

        public void SetRoles(ApplicationDbContext context)
        {
            context.Roles.Add(AdminRole);
            context.Roles.Add(OperatorCedacriRole);
            context.Roles.Add(UserBancarRole);
        }
        #endregion

        #region Users
        public static readonly User Admin = new User
        {
            Id = 1,
            UserName = "Cr00001",
            Password = "e17c8fa0a351caf1138741f0862208a250ecfa122ce3f4cbba637a2e510e2920",
            Name = "Admin1",
            Surname = "Admin1",
            Patronymic = "Admin1",
            Email = "admin1@mail.com",
            IsEnabled = true,
        };
        public static readonly User MainUserAdmin = new User
        {
            Id = 2,
            UserName = "Crme145",
            Password = "e17c8fa0a351caf1138741f0862208a250ecfa122ce3f4cbba637a2e510e2920", //Cedacri1234567!
            Name = "Admin",
            Surname = "Admin",
            Patronymic = "Admin",
            Email = "admin@mail.com",
            IsEnabled = true,
        };

        public void SetUsers(ApplicationDbContext context)
        {
            context.Users.Add(Admin);
            context.Users.Add(MainUserAdmin);
        }

        #endregion

        #region UserRole
        public static readonly UserRole RoleAdmin = new UserRole { UserId = Admin.Id, RoleId = AdminRole.Id };
        public static readonly UserRole RoleMainAdmin = new UserRole { UserId = MainUserAdmin.Id, RoleId = AdminRole.Id };
        public void SetUserRoles(ApplicationDbContext context)
        {
            context.UserRoles.Add(RoleAdmin);
            context.UserRoles.Add(RoleMainAdmin);
        }
        #endregion


        public DbInitializer()
        {
            this.AutomaticMigrationDataLossAllowed = true;
            this.AutomaticMigrationsEnabled = true;
        }
        protected override void Seed(ApplicationDbContext context)
        {
            if(!context.Roles.Any())
            {
                SetRoles(context);
            }
            if (!context.Users.Any())
            {
                SetUsers(context);
            }
            if (!context.UserRoles.Any())
            {
                SetUserRoles(context);
            }
            base.Seed(context);
            context.SaveChanges();

        }
    }

}
