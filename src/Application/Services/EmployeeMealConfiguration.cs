using Application.DTOs;
using Application.Interfaces;
using Domain.Entitys.MealModel;
using Domain.Interfaces.Generic;

namespace Application.Services
{
    public class EmployeeMealConfiguration: IEmployeeMealConfiguration
    {
        private readonly IRepository<EmployeeMealDay> _employeeMealRepository;
        public EmployeeMealConfiguration(IRepository<EmployeeMealDay> employeeMealRepository)
        {
            _employeeMealRepository = employeeMealRepository;
        }
        public async Task<EmployeeMealDayDto> GetByIdAsync(int id)
        {
           var result=await _employeeMealRepository.GetByIdAsync(id);
            if (result == null)
                return new EmployeeMealDayDto();
            return new EmployeeMealDayDto
            {
                Id = result.Id,
                Breakfast= result.Breakfast,
                BreakfastGuest = result.BreakfastGuest,
                Lunch = result.Lunch,
                LunchGuest = result.LunchGuest,
                Dinner = result.Dinner,
                DinnerGuest = result.DinnerGuest,
                IsSundayMeal = result.IsSundayMeal,
                IsMondayMeal = result.IsMondayMeal,
                IsTuesdayMeal = result.IsTuesdayMeal,
                IsWednesdayMeal = result.IsWednesdayMeal,
                IsThursdayMeal = result.IsThursdayMeal,
                IsFridayMeal = result.IsFridaydayMeal,
                IsSaturdayMeal = result.IsSaturdayMeal,
                UserId = result.UserId
            };

        }
        public async Task<List<EmployeeMealDayDto>> GetAllAsync()
        {
            var meals=await _employeeMealRepository.GetAllIncludingAsync(x => x.User);
            return meals.Select(meal => new EmployeeMealDayDto
            {
                Id=meal.Id,
                Breakfast = meal.Breakfast,
                BreakfastGuest = meal.BreakfastGuest,
                Lunch = meal.Lunch,
                LunchGuest = meal.LunchGuest,
                Dinner = meal.Dinner,
                DinnerGuest = meal.DinnerGuest,
                IsSundayMeal = meal.IsSundayMeal,
                IsMondayMeal = meal.IsMondayMeal,
                IsTuesdayMeal = meal.IsTuesdayMeal,
                IsWednesdayMeal = meal.IsWednesdayMeal,
                IsThursdayMeal = meal.IsThursdayMeal,
                IsFridayMeal = meal.IsFridaydayMeal,
                IsSaturdayMeal = meal.IsSaturdayMeal,
                UserId = meal.UserId,
                UserName = meal.User?.EmployeeName ?? "",
                EmployeeId = meal.User?.EmployeeId ?? 0,    
            }).ToList();
        }
        public async Task<EmployeeMealDayDto> CreateAsync(EmployeeMealDayDto meal)
        {
            var employeeMeal=new EmployeeMealDay
            {
                Breakfast = meal.Breakfast,
                BreakfastGuest = meal.BreakfastGuest,
                Lunch = meal.Lunch,
                LunchGuest = meal.LunchGuest,
                Dinner = meal.Dinner,
                DinnerGuest = meal.DinnerGuest,
                IsSundayMeal = meal.IsSundayMeal,
                IsMondayMeal = meal.IsMondayMeal,
                IsTuesdayMeal = meal.IsTuesdayMeal,
                IsWednesdayMeal = meal.IsWednesdayMeal,
                IsThursdayMeal = meal.IsThursdayMeal,
                IsFridaydayMeal = meal.IsFridayMeal,
                IsSaturdayMeal = meal.IsSaturdayMeal,
                UserId = meal.UserId
            };
            await _employeeMealRepository.AddAsync(employeeMeal);
            return meal;
        }
        public async Task UpdateAsync(EmployeeMealDayDto meal)
        {
            var employeeMeal = new EmployeeMealDay
            {
                Breakfast = meal.Breakfast,
                BreakfastGuest = meal.BreakfastGuest,
                Lunch = meal.Lunch,
                LunchGuest = meal.LunchGuest,
                Dinner = meal.Dinner,
                DinnerGuest = meal.DinnerGuest,
                IsSundayMeal = meal.IsSundayMeal,
                IsMondayMeal = meal.IsMondayMeal,
                IsTuesdayMeal = meal.IsTuesdayMeal,
                IsWednesdayMeal = meal.IsWednesdayMeal,
                IsThursdayMeal = meal.IsThursdayMeal,
                IsFridaydayMeal = meal.IsFridayMeal,
                IsSaturdayMeal = meal.IsSaturdayMeal,
                UserId = meal.UserId
            };
            await _employeeMealRepository.UpdateAsync(employeeMeal);
        }
        public async Task DeleteAsync(int id)
        {
            var meal = await _employeeMealRepository.GetByIdAsync(id);
            if (meal != null)
            {
                await _employeeMealRepository.DeleteAsync(meal);
            }
        }
    }
}
