
namespace Application.DTOs
{
    public class DailyMealDto
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public DateTime Date { get; set; }

        public double Breakfast { get; set; }
        public double BreakfastGuest { get; set; }
        public bool IsBreakfastComplete { get; set; }

        public double Lunch { get; set; }
        public double LunchGuest { get; set; }
        public bool IsLunchComplete { get; set; }

        public double Dinner { get; set; }
        public double DinnerGuest { get; set; }
        public bool IsDinnerComplete { get; set; }
    }
}
