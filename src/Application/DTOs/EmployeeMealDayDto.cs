
namespace Application.DTOs
{
    public class EmployeeMealDayDto
    {
        public int? Id { get; set; }
        public double? Breakfast { get; set; }
        public double? BreakfastGuest { get; set; }
        public double? Lunch { get; set; }
        public double? LunchGuest { get; set; }
        public double? Dinner { get; set; }
        public double? DinnerGuest { get; set; }

        public bool? IsSundayMeal { get; set; }
        public bool? IsMondayMeal { get; set; }
        public bool? IsTuesdayMeal { get; set; }
        public bool? IsWednesdayMeal { get; set; }
        public bool? IsThursdayMeal { get; set; }
        public bool? IsFridayMeal { get; set; }
        public bool? IsSaturdayMeal { get; set; }

        public int? UserId { get; set; }
        public int? EmployeeId { get; set; }
        public string? UserName { get; set; }
    }
}
