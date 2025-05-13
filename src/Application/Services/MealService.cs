using Application.Interfaces;
using Domain.Entitys.MealModel;
using Domain.Interfaces.Meal;
using Domain.ViewModel;
using System.Globalization;

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

        public async Task<bool> ExistsForEmployeeAsync(int employeeId, int year, int month, int day)
        {
            return await _mealRepository.ExistsForEmployeeAsync(employeeId,year,month,day);
        }

        public async Task<IReadOnlyList<DailyMeal>> MealForDayAsync(int year, int month, int day)
        {
            return await _mealRepository.MealForDayAsync(year,month,day);
        }

        public async Task<DailyMeal> MealForDayByEmployeeAsync(int employeeId, int year, int month, int day)
        {
            return await _mealRepository.MealForDayByEmployeeAsync(employeeId,year,month,day);
        }

        public async Task<IReadOnlyList<DailyMeal>> MealForMonthAsync(int year, int month)
        {
            return await _mealRepository.MealForMonthAsync(year, month);
        }

        public async Task<IReadOnlyList<DailyMeal>> MealForMonthByEmployeeAsync(int employeeId, int year, int month)
        {
            return await _mealRepository.MealForMonthByEmployeeAsync(employeeId,year,month);
        }

        public async Task<bool> SetOffDayMealsAsync(string weekend, int month, int year, int employeeId)
        {
            return await _mealRepository.SetOffDayMealsAsync(weekend, month, year, employeeId);
        }

        public async Task<bool> UpdateDailyMealByProperty(int id, string propertyName, double value)
        {
            propertyName=propertyName.First().ToString().ToUpper() + propertyName.Substring(1);
            //propertyName= new CultureInfo("en-US").TextInfo.ToTitleCase(propertyName);
            return await _mealRepository.UpdateDailyMealByProperty(id, propertyName, value);
        }

        public async Task<bool> UpdateMonthlyMealByProperty(int empId, int month, int year, string propertyName, double value)
        {
            //propertyName = new CultureInfo("en-US").TextInfo.ToTitleCase(propertyName);
            propertyName = propertyName.First().ToString().ToUpper() + propertyName.Substring(1);

            return await _mealRepository.UpdateMonthlyMealByProperty(empId, month, year, propertyName, value);
        }
    }
}
