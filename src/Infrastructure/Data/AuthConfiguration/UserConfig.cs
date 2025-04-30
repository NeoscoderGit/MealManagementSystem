
using Domain.Entitys.AuthModel;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.AuthConfiguration
{
    public class UserConfig : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(u => u.Id);

            builder.Property(u => u.EmployeeName)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(u => u.EmployeeCode)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.Property(u => u.OfficeEmail)
                   .HasMaxLength(150);

            builder.HasMany(u => u.UserRoles)
                   .WithOne(ur => ur.User)
                   .HasForeignKey(ur => ur.UserId);
            builder.Property(e => e.CreatedDate).HasDefaultValueSql("GETDATE()").ValueGeneratedOnAdd();
        }
    }
}
