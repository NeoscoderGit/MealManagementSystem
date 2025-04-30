
using Domain.Entitys.AuthModel;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.AuthConfiguration
{
    public class MenuConfig : IEntityTypeConfiguration<Menu>
    {
        public void Configure(EntityTypeBuilder<Menu> builder)
        {
            builder.HasKey(m => m.Id);

            builder.Property(m => m.Name)
                .HasMaxLength(100)
                .IsRequired(false);

            builder.Property(m => m.Url)
                .HasMaxLength(200)
                .IsRequired(false);
            builder.Property(e => e.CreatedDate).HasDefaultValueSql("GETDATE()").ValueGeneratedOnAdd();

            // Self-referencing Parent - Children relationship
            builder
                .HasOne(m => m.Parent)
                .WithMany(m => m.Children)
                .HasForeignKey(m => m.ParentId)
                .OnDelete(DeleteBehavior.Restrict); // 🔥 Important: RESTRICT, not CASCADE

            // RoleMenus relationship (delete role-menu when menu deleted)
            builder
                .HasMany(m => m.RoleMenus)
                .WithOne(rm => rm.Menu)
                .HasForeignKey(rm => rm.MenuId)
                .OnDelete(DeleteBehavior.Cascade); // ✅ cascade delete for dependent tables only
        }
    }

}
