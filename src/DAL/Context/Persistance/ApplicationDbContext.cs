using DAL.Context.Configuration;
using DAL.Entities;
using System.Data.Entity;

namespace DAL.Context.Persistance
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<Institution> Institutions { get; set; }

        public ApplicationDbContext() : base("name=DefaultConnection")
        {
            Database.SetInitializer<ApplicationDbContext>(new DbInitializer());
            //Database.SetInitializer(new DbInitializer());
        }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Configurations.Add(new RoleConfiguration());
            modelBuilder.Configurations.Add(new UserConfiguration());
            modelBuilder.Configurations.Add(new UserRoleConfiguration());

        }

    }
}
