using Domain.Entitys.MealModel;
using Domain.Interfaces.Generic;
using Domain.ViewModel;

namespace Domain.Interfaces.Meal
{
    public interface IDailyMealRepository : IRepository<DailyMeal>
    {
        Task<bool> ExistsForEmployeeAsync(int employeeId, int year, int month, int day);
        Task GenerateMonthlyMealsAsync(int year, int month, IEnumerable<int> employeeIds);
        Task<DailyMeal> MealForDayByEmployeeAsync(int employeeId, int year, int month, int day);
        Task<IReadOnlyList<DailyMeal>> MealForMonthByEmployeeAsync(int employeeId, int year, int month);
        Task<IReadOnlyList<DailyMeal>> MealForMonthAsync(int year, int month);
        Task<IReadOnlyList<DailyMeal>> MealForDayAsync(int year, int month, int day);
        Task<MealCheckResultViewModel> AutoDetectAndCheckMealAsync(string cardNumber, DateTime requestDateTime);
        Task<bool> UpdateDailyMealByProperty(int id,string propertyName,double value);
        Task<bool> UpdateMonthlyMealByProperty(int empId,int month,int year,string propertyName,double value);
        Task<bool> SetOffDayMealsAsync(string weekend,int month,int year, int employeeId);
    }
}
