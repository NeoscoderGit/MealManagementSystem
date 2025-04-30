
using Domain.Entitys.AuthModel;
using Domain.Entitys.Common;

namespace Domain.Entitys.MealModel
{
    public class DailyMeal:BaseEntity
    {
        public double? Breakfast { get; set; } = 1.0;
        public double? BreakfastGuest { get; set; } = 0.0;
        public double? BreakTotalTaken{ get; set; } = 0.0;       
        public bool? IsBreakfastComplete { get; set; } = false;  // Preventing update after meal-day, on meal day it should be true.
        public bool? IsBreakfastTaken { get; set; } = false;  //True after total meal taken.


        public double? Lunch { get; set; } = 1.0;
        public double? LunchGuest { get; set; } = 0.0;
        public double? LunchTotalTaken { get; set; } = 0.0;
        public bool? IsLunchTaken { get; set; } = false;
        public bool? IsLunchComplete { get; set; } = false;


        public double? Dinner { get; set; } = 1.0;
        public double? DinnerGuest { get; set; } = 0.0;
        public double? DinnerTotalTaken { get; set; } = 0.0;
        public bool? IsDinnerComplete { get; set; } = false;
        public bool? IsDinnerTaken { get; set; } = false;


        public int Month { get; set; }
        public int Year{ get; set; }
        public int Day { get; set; }
        public DateTime Date { get; private set; }

        // Navigation property
        public int EmployeeId { get; set; }
        public User? Employee { get; set; }
    }
}
