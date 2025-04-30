using Application.Interfaces;
using Domain.Interfaces.Meal;

namespace Application.Services
{
    public class MealGeneratorService: IMealGeneratorService
    {
        private readonly IDailyMealRepository _mealRepo;
        private readonly IUserService _employeeRepo;

        public MealGeneratorService(
            IDailyMealRepository mealRepo,
            IUserService employeeRepo)
        {
            _mealRepo = mealRepo;
            _employeeRepo = employeeRepo;
        }

        public async Task GenerateMonthlyMealsAsync(int year, int month)
        {
            try
            {
                var activeEmployees = await _employeeRepo.GetAllAsync();
                var employeeIds = activeEmployees.Select(e => e.Id);

                await _mealRepo.GenerateMonthlyMealsAsync(year, month, employeeIds);
                //_logger.LogInformation($"Generated meals for {month}/{year}");
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, "Meal generation failed");
                throw;
            }
        }
    }
}
