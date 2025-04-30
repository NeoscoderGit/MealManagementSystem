
using Domain.Entitys.AuthModel;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.AuthConfiguration
{
    public class RoleMenuConfig : IEntityTypeConfiguration<RoleMenu>
    {
        public void Configure(EntityTypeBuilder<RoleMenu> builder)
        {
            builder.HasKey(rm => rm.Id);

            builder.HasOne(rm => rm.Role)
                   .WithMany(r => r.RoleMenus)
                   .HasForeignKey(rm => rm.RoleId);

            builder.HasOne(rm => rm.Menu)
                   .WithMany(m => m.RoleMenus)
                   .HasForeignKey(rm => rm.MenuId);
            builder.Property(e => e.CreatedDate).HasDefaultValueSql("GETDATE()").ValueGeneratedOnAdd();
        }
    }

}
