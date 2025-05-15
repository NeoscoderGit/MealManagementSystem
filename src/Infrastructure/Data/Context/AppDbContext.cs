using Domain.Entitys.AuthModel;
using Domain.Entitys.Common;
using Domain.Entitys.MealModel;
using Infrastructure.Data.AuthConfiguration;
using Infrastructure.Data.MealConfiguration;
using Infrastructure.Data.Seed.AuthSeed;
using Infrastructure.Data.Seed.MealSeed;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new UserConfig());
            modelBuilder.ApplyConfiguration(new UserRoleConfig());
            modelBuilder.ApplyConfiguration(new RoleConfig());
            modelBuilder.ApplyConfiguration(new RoleMenuConfig());
            modelBuilder.ApplyConfiguration(new MenuConfig());


            modelBuilder.ApplyConfiguration(new DailyMealConfiguration());
            modelBuilder.ApplyConfiguration(new EmployeeMealDayConfiguration());

            AuthDataSeed.Seed(modelBuilder);
            MealDataSeed.Seed(modelBuilder);
        }

        public override int SaveChanges()
        {
            ApplyAuditInfo();
            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            ApplyAuditInfo();
            return await base.SaveChangesAsync(cancellationToken);
        }

        private void ApplyAuditInfo()
        {
            foreach (var entry in ChangeTracker.Entries<BaseEntity>())
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedDate = DateTime.Now;
                }
                else if (entry.State == EntityState.Modified)
                {
                    entry.Property(e => e.CreatedDate).IsModified = false; // protect original date
                    entry.Entity.UpdatedDate = DateTime.Now;
                }
            }
        }

        //Auth
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<Menu> Menus { get; set; }
        public DbSet<RoleMenu> RoleMenus { get; set; }

        //Meal
        public DbSet<MealTime> MealTimes { get; set; }
        public DbSet<DailyMeal> DailyMeals { get; set; }
        public DbSet<EmployeeMealDay> EmployeeMealDays { get; set; }
    }
}
