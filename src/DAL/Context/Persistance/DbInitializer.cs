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
        private static readonly Role AdminRole = new Role { Id = 1, Name = "Administrator" };
        private static readonly Role OperatorCedacriRole = new Role { Id = 2, Name = "Operator Cedacri" };
        private static readonly Role UserBancarRole = new Role { Id = 3, Name = "Operator Bancar" };

        private void SetRoles(ApplicationDbContext context)
        {
            context.Roles.Add(AdminRole);
            context.Roles.Add(OperatorCedacriRole);
            context.Roles.Add(UserBancarRole);
        }
        #endregion

        #region Users
        private static readonly User Admin = new User
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
        private static readonly User MainUserAdmin = new User
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

        private void SetUsers(ApplicationDbContext context)
        {
            context.Users.Add(Admin);
            context.Users.Add(MainUserAdmin);
        }

        #endregion

        #region UserRole
        private static readonly UserRole RoleAdmin = new UserRole { UserId = Admin.Id, RoleId = AdminRole.Id };
        private static readonly UserRole RoleMainAdmin = new UserRole { UserId = MainUserAdmin.Id, RoleId = AdminRole.Id };
        private void SetUserRoles(ApplicationDbContext context)
        {
            context.UserRoles.Add(RoleAdmin);
            context.UserRoles.Add(RoleMainAdmin);
        }
        #endregion



        #region DocumentTypes
        private static readonly DocumentType DocumentType1 = new DocumentType { Id = 1, Code = "RPSRV", Name = "Report di Servizio", TypeDscr = "Report SLA per periodo", IsMacro = true, IsDateGrouped = true  };
        private static readonly DocumentType DocumentType2 = new DocumentType { Id = 2, Code = "RPSLA", Name = "Report SLA", TypeDscr = "Report SLA per periodo", IsMacro = true, IsDateGrouped = true  };
        private static readonly DocumentType DocumentType3 = new DocumentType { Id = 3, Code = "RPPRG", Name = "Report di Projectto", TypeDscr = "Documentazione per progetti", IsMacro = true, IsDateGrouped = false };
        private static readonly DocumentType DocumentType4 = new DocumentType { Id = 4, Code = "NETWK", Name = "Network", TypeDscr = "Network", IsMacro = false, IsDateGrouped = false };
        private static readonly DocumentType DocumentType5 = new DocumentType { Id = 5, Code = "SICRZ", Name = "Sicurezza", TypeDscr = "Sicurezza", IsMacro = false, IsDateGrouped = false };
        private static readonly DocumentType DocumentType6 = new DocumentType { Id = 6, Code = "CHNGE", Name = "Change", TypeDscr = "Change", IsMacro = false, IsDateGrouped = false };
        private static readonly DocumentType DocumentType7 = new DocumentType { Id = 7, Code = "BCKUP", Name = "Backup", TypeDscr = "Backup", IsMacro = false, IsDateGrouped = false };
        private static readonly DocumentType DocumentType8 = new DocumentType { Id = 8, Code = "ANLIS", Name = "Analisi", TypeDscr = "Analisi", IsMacro = false, IsDateGrouped = false };
        private static readonly DocumentType DocumentType9 = new DocumentType { Id = 9, Code = "TRNSZ", Name = "Transizione", TypeDscr = "Transizione", IsMacro = false, IsDateGrouped = false };
        private static readonly DocumentType DocumentType10 = new DocumentType { Id = 10, Code = "PRDZN", Name = "Produzione", TypeDscr = "Produzione", IsMacro = false, IsDateGrouped = false };
        private static readonly DocumentType DocumentType11 = new DocumentType { Id = 11, Code = "TEST", Name = "Test", TypeDscr = "Test", IsMacro = false, IsDateGrouped = false };
        private static readonly DocumentType DocumentType12 = new DocumentType { Id = 12, Code = "MNTRG", Name = "Monitoraggio", TypeDscr = "Monitoraggio", IsMacro = false, IsDateGrouped = false };
        private void SetDocumentTypes(ApplicationDbContext context)
        {
            context.DocumentTypes.AddOrUpdate(DocumentType1);
            context.DocumentTypes.AddOrUpdate(DocumentType2);
            context.DocumentTypes.AddOrUpdate(DocumentType3);
            context.DocumentTypes.AddOrUpdate(DocumentType4);
            context.DocumentTypes.AddOrUpdate(DocumentType5);
            context.DocumentTypes.AddOrUpdate(DocumentType6);
            context.DocumentTypes.AddOrUpdate(DocumentType7);
            context.DocumentTypes.AddOrUpdate(DocumentType8);
            context.DocumentTypes.AddOrUpdate(DocumentType9);
            context.DocumentTypes.AddOrUpdate(DocumentType10);
            context.DocumentTypes.AddOrUpdate(DocumentType11);
            context.DocumentTypes.AddOrUpdate(DocumentType12);
        }
        #endregion

        #region DocumentTypeHierarchies
        private static readonly DocumentTypeHierarchy DocumentTypeHierarchy1 = new DocumentTypeHierarchy { IdMacro = 1, IdMicro = 4};
        private static readonly DocumentTypeHierarchy DocumentTypeHierarchy2 = new DocumentTypeHierarchy { IdMacro = 1, IdMicro = 5 };
        private static readonly DocumentTypeHierarchy DocumentTypeHierarchy3 = new DocumentTypeHierarchy { IdMacro = 1, IdMicro = 6 };
        private static readonly DocumentTypeHierarchy DocumentTypeHierarchy4 = new DocumentTypeHierarchy { IdMacro = 1, IdMicro = 7 };
        private static readonly DocumentTypeHierarchy DocumentTypeHierarchy5 = new DocumentTypeHierarchy { IdMacro = 3, IdMicro = 8 };
        private static readonly DocumentTypeHierarchy DocumentTypeHierarchy6 = new DocumentTypeHierarchy { IdMacro = 3, IdMicro = 9 };
        private static readonly DocumentTypeHierarchy DocumentTypeHierarchy7 = new DocumentTypeHierarchy { IdMacro = 3, IdMicro = 10 };
        private static readonly DocumentTypeHierarchy DocumentTypeHierarchy8 = new DocumentTypeHierarchy { IdMacro = 3, IdMicro = 11 };
        private static readonly DocumentTypeHierarchy DocumentTypeHierarchy9 = new DocumentTypeHierarchy { IdMacro = 3, IdMicro = 12 };
        private static readonly DocumentTypeHierarchy DocumentTypeHierarchy10 = new DocumentTypeHierarchy { IdMacro = 2, IdMicro = 2 };
       
        private void SetDocumentTypeHierarchies(ApplicationDbContext context)
        {
            context.DocumentTypeHierarchies.Add(DocumentTypeHierarchy1);
            context.DocumentTypeHierarchies.Add(DocumentTypeHierarchy2);
            context.DocumentTypeHierarchies.Add(DocumentTypeHierarchy3);
            context.DocumentTypeHierarchies.Add(DocumentTypeHierarchy4);
            context.DocumentTypeHierarchies.Add(DocumentTypeHierarchy5);
            context.DocumentTypeHierarchies.Add(DocumentTypeHierarchy6);
            context.DocumentTypeHierarchies.Add(DocumentTypeHierarchy7);
            context.DocumentTypeHierarchies.Add(DocumentTypeHierarchy8);
            context.DocumentTypeHierarchies.Add(DocumentTypeHierarchy9);
            context.DocumentTypeHierarchies.Add(DocumentTypeHierarchy10);
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


            if (!context.DocumentTypes.Any())
            {
                SetDocumentTypes(context);
            }
            if (!context.DocumentTypeHierarchies.Any())
            {
                SetDocumentTypeHierarchies(context);
            }

            base.Seed(context);
            context.SaveChanges();

        }
    }

}
