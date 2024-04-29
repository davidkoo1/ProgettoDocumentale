using DAL.Entities;
using System.Data.Entity.ModelConfiguration;

namespace DAL.Context.Configuration
{
    public class UserConfiguration : EntityTypeConfiguration<User>
    {
        public UserConfiguration()
        {
            HasKey(user => user.Id);

            Property(user => user.UserName)
                .HasMaxLength(7)
                .IsRequired();

            HasIndex(user => user.UserName)
                .IsUnique();

            Property(user => user.Email)
                .HasMaxLength(150)
                .IsRequired();

            HasIndex(user => user.Email)
                .IsUnique();

            Property(user => user.IsEnabled)
                .IsRequired();

            Property(user => user.Name)
                .HasMaxLength(100)
                .IsRequired();

            Property(user => user.Surname)
                .HasMaxLength(100)
                .IsRequired();

            Property(user => user.Patronymic)
                .HasMaxLength(100)
                .IsRequired();
        }
    }

}
