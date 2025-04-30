using Domain.Entitys.MealModel;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.MealConfiguration
{
    public class DailyMealConfiguration : IEntityTypeConfiguration<DailyMeal>
    {
        public void Configure(EntityTypeBuilder<DailyMeal> builder)
        {
            builder.ToTable("DailyMeals");

            // Required date fields
            builder.Property(x => x.Year).IsRequired();
            builder.Property(x => x.Month).IsRequired();
            builder.Property(x => x.Day).IsRequired();
            

            // Computed Date column (SQL Server syntax)
            builder.Property(x => x.Date).HasComputedColumnSql("DATEFROMPARTS([Year], [Month], [Day])", stored: true);

            // Relationships
            builder.HasOne(x => x.Employee)
                   .WithMany()
                   .HasForeignKey(x => x.EmployeeId)
                   .OnDelete(DeleteBehavior.Restrict);

            // Indexes (including one for the computed Date column)
            builder.HasIndex(x => x.EmployeeId);
            builder.HasIndex(x => x.Date);  // Index on computed column

            builder.HasIndex(d => new { d.EmployeeId, d.Year, d.Month, d.Day }).IsUnique();
            builder.HasIndex(d => new { d.EmployeeId, d.Year, d.Month, d.Day });
            builder.HasIndex(d => new { d.Year, d.Month, d.Day });
            builder.HasIndex(d => new { d.Year, d.Month });

            // Constraints
            builder.HasCheckConstraint(
                "CK_DailyMeal_ValidDate",
                "Year BETWEEN 2000 AND 2100 AND " +
                "Month BETWEEN 1 AND 12 AND " +
                "Day BETWEEN 1 AND 31 AND " +
                "(Day <= CASE " +
                "    WHEN Month IN (1,3,5,7,8,10,12) THEN 31 " +
                "    WHEN Month IN (4,6,9,11) THEN 30 " +
                "    WHEN Year % 4 = 0 AND (Year % 100 != 0 OR Year % 400 = 0) THEN 29 " +
                "    ELSE 28 " +
                "END)");  // Ensures valid date

            builder.HasCheckConstraint(
                "CK_DailyMeal_PositiveMeals",
                "(Breakfast IS NULL OR Breakfast >= 0) AND " +
                "(Lunch IS NULL OR Lunch >= 0) AND " +
                "(Dinner IS NULL OR Dinner >= 0) AND " +
                "(BreakfastGuest IS NULL OR BreakfastGuest >= 0) AND " +
                "(LunchGuest IS NULL OR LunchGuest >= 0) AND " +
                "(DinnerGuest IS NULL OR DinnerGuest >= 0)");

            builder.Property(e => e.CreatedDate).HasDefaultValueSql("GETDATE()").ValueGeneratedOnAdd();
        }
    }
}
