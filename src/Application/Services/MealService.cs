using Application.Interfaces;
using Domain.Entitys.MealModel;
using Domain.Interfaces.Meal;
using Domain.ViewModel;

namespace Application.Services
{
    public class MealService : IMealService
    {
        private readonly IDailyMealRepository _mealRepository;

        public MealService(IDailyMealRepository mealRepository)
        {
            _mealRepository = mealRepository;
        }

        public async Task<MealCheckResultViewModel?> AutoDetectAndCheckMealAsync(string cardNumber, DateTime requestDateTime)
        {
           return await _mealRepository.AutoDetectAndCheckMealAsync(cardNumber,requestDateTime);
        }

        public Task<bool> ExistsForEmployeeAsync(int employeeId, int year, int month, int day)
        {
            return _mealRepository.ExistsForEmployeeAsync(employeeId,year,month,day);
        }

        public Task<IReadOnlyList<DailyMeal>> MealForDayAsync(int year, int month, int day)
        {
            return _mealRepository.MealForDayAsync(year,month,day);
        }

        public Task<DailyMeal> MealForDayByEmployeeAsync(int employeeId, int year, int month, int day)
        {
            return _mealRepository.MealForDayByEmployeeAsync(employeeId,year,month,day);
        }

        public Task<IReadOnlyList<DailyMeal>> MealForMonthAsync(int year, int month)
        {
            return _mealRepository.MealForMonthAsync(year, month);
        }

        public Task<IReadOnlyList<DailyMeal>> MealForMonthByEmployeeAsync(int employeeId, int year, int month)
        {
            return _mealRepository.MealForMonthByEmployeeAsync(employeeId,year,month);
        }
    }
}
