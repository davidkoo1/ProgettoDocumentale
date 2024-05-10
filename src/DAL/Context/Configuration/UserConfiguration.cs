using DAL.Entities;
using System.Data.Entity.ModelConfiguration;
using System.Reflection;

namespace DAL.Context.Configuration
{
    public class UserConfiguration : EntityTypeConfiguration<User>
    {
        public UserConfiguration()
        {
            HasKey(user => user.Id);

            Property(user => user.UserName) 
                .HasMaxLength(32) 
                .IsRequired();

            Property(user => user.Name)
                .HasMaxLength(100)
                .IsRequired();

            Property(user => user.Surname)
                .HasMaxLength(100)
                .IsRequired();

            Property(user => user.Patronymic)
                .HasMaxLength(100)
                .IsOptional();

            Property(user => user.Password) 
                .HasMaxLength(256) 
                .IsRequired();

            Property(user => user.Email) 
                .HasMaxLength(150)
                .IsRequired();

            Property(user => user.IsEnabled) 
                .IsRequired();

            Property(user => user.IdInstitution).IsOptional();

            HasOptional(user => user.Institution)
                .WithMany(institution => institution.Users) 
                .HasForeignKey(user => user.IdInstitution); 


            // Indexes
            HasIndex(user => user.UserName)
                .IsUnique();
            HasIndex(user => user.Email)
                .IsUnique();
        }
    }

}
