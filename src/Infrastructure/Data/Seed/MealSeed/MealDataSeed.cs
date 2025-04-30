using Domain.Entitys.MealModel;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Seed.MealSeed
{
    public static class MealDataSeed
    {
        public static void Seed(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MealTime>().HasData(
                new MealTime
                {
                    Id = 1, // Make sure this ID doesn't conflict with existing data
                    BreakFastTimeStart = new TimeOnly(8, 0),
                    BreakFastTimeEnd = new TimeOnly(11, 0),
                    LunchTimeStart = new TimeOnly(12, 0),
                    LunchTimeEnd = new TimeOnly(16, 0),
                    DinnerTimeStart = new TimeOnly(19, 0),
                    DinnerTimeEnd = new TimeOnly(23, 0)
                }
            );
        }
    }
}
