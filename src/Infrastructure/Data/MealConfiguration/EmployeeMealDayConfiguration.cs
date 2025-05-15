
using Domain.Entitys.MealModel;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.MealConfiguration
{
    public class EmployeeMealDayConfiguration : IEntityTypeConfiguration<EmployeeMealDay>
    {
        public void Configure(EntityTypeBuilder<EmployeeMealDay> builder)
        {
            builder.ToTable("EmployeeMealDays");
            builder.HasKey(e => e.Id);
            builder.Property(e => e.IsSundayMeal).HasDefaultValue(true);
            builder.Property(e => e.IsMondayMeal).HasDefaultValue(true);
            builder.Property(e => e.IsTuesdayMeal).HasDefaultValue(true);
            builder.Property(e => e.IsWednesdayMeal).HasDefaultValue(true);
            builder.Property(e => e.IsThursdayMeal).HasDefaultValue(true);
            builder.Property(e => e.IsFridaydayMeal).HasDefaultValue(true);
            builder.Property(e => e.IsSaturdayMeal).HasDefaultValue(true);

            // Relationship: EmployeeMealDay → User
            builder.HasOne(e => e.User)
                   .WithMany()
                   .HasForeignKey(e => e.UserId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
