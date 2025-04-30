
namespace Domain.ViewModel
{
    public class MealCheckResultViewModel
    {
        public string UserName { get; set; } = string.Empty;
        public int EmployeeID { get; set; }
        public DateTime Date { get; set; }
        public string MealPeriod { get; set; } = string.Empty; // Breakfast, Lunch, Dinner
        public double TotalMeal { get; set; } // Breakfast + Guest
        public TimeOnly Time { get; set; }
        public bool IsAvailable { get; set; }
        public string Message { get; set; } = string.Empty;
    }

}
