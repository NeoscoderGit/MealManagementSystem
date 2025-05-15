using Domain.Entitys.AuthModel;
using Domain.Entitys.Common;

namespace Domain.Entitys.MealModel
{
    public class EmployeeMealDay:BaseEntity
    {
        public double? Breakfast{ get; set; }
        public double? BreakfastGuest { get; set; }
        public double? Lunch { get; set; }
        public double? LunchGuest { get; set; }
        public double? Dinner{ get; set; }
        public double? DinnerGuest { get; set; }

        public bool? IsSundayMeal { get; set; }
        public bool? IsMondayMeal { get; set; }
        public bool? IsTuesdayMeal { get; set; }
        public bool? IsWednesdayMeal { get; set; }
        public bool? IsThursdayMeal { get; set; }
        public bool? IsFridaydayMeal { get; set; }
        public bool? IsSaturdayMeal { get; set; }

        public int? UserId { get; set; }
        public User? User { get; set; }
    }
}
