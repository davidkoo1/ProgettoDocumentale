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
        public DbSet<Document> Documents { get; set; }
        public DbSet<DocumentType> DocumentTypes { get; set; }
        public DbSet<DocumentTypeHierarchy> DocumentTypeHierarchies { get; set; }
        public DbSet<Project> Projects { get; set; }

        public ApplicationDbContext() : base("name=DefaultConnection")
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<ApplicationDbContext, DbInitializer>());
            //Database.SetInitializer<ApplicationDbContext>(new DbInitializer());
            //Database.SetInitializer(new DbInitializer());
        }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Configurations.Add(new RoleConfiguration());
            modelBuilder.Configurations.Add(new UserConfiguration());
            modelBuilder.Configurations.Add(new UserRoleConfiguration());

            modelBuilder.Configurations.Add(new DocumentConfiguration());
            modelBuilder.Configurations.Add(new DocumentTypeConfiguration());
            modelBuilder.Configurations.Add(new DocumentTypeHierarchyConfiguration());

            modelBuilder.Configurations.Add(new ProjectConfiguration());

        }

    }
}
