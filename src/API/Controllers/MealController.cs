using Application.DTOs;
using Application.Interfaces;
using Application.Services.BackgroundServices;
using Domain.Entitys.MealModel;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/meals")]
public class MealController : ControllerBase
{
    private readonly IMealGeneratorService _generator;
    private readonly MealStatusUpdaterService _updaterService;
    private readonly IMealService _mealService;

    public MealController(IMealGeneratorService generator, IMealService mealService,MealStatusUpdaterService updaterService)
    {
        _generator = generator;
        _mealService = mealService;
        _updaterService = updaterService;
    }

    // GET api/dailymeals/get-monthly-meal-by-employeeId/{employeeId}/month/{year}-{month}-{day}
    [HttpGet("get-daily-meal-by-employeeId/{employeeId}/date/{year}-{month}-{day}")]
    public async Task<ActionResult<DailyMeal>> GetEmployeeDailyMeal(int employeeId, int year, int month, int day)
    {
        try
        {
            var meal = await _mealService.MealForDayByEmployeeAsync(employeeId, year, month, day);

            if (meal == null)
                return NotFound("Meal record not found");

            return Ok(meal);
        }
        catch (Exception ex)
        {
            //_logger.LogError(ex, $"Error getting daily meal for employee {employeeId}");
            return StatusCode(500, "Internal server error");
        }
    }

    // GET api/dailymeals/get-monthly-meal-by-employeeId/{employeeId}/month/{year}-{month}
    [HttpGet("get-monthly-meal-by-employee/{employeeId}/month/{year}-{month}")]
    public async Task<ActionResult<IReadOnlyList<DailyMeal>>> GetEmployeeMonthlyMeals(int employeeId, int year, int month)
    {
        try
        {
            var meals = await _mealService.MealForMonthByEmployeeAsync(employeeId, year, month);
            return Ok(meals.ToList());
        }
        catch (Exception ex)
        {
            //_logger.LogError(ex, $"Error getting monthly meals for employee {employeeId}");
            return StatusCode(500, "Internal server error");
        }
    }

    // GET api/dailymeals/get-daily-meal/{year}-{month}-{day}
    [HttpGet("get-daily-meal/{year}-{month}-{day}")]
    public async Task<ActionResult<IReadOnlyList<DailyMealDto>>> GetDailyMealsForAllEmployees(int year, int month, int day)
    {
        try
        {
            var meals = await _mealService.MealForDayAsync(year, month, day);
            return Ok(meals.ToList());
        }
        catch (Exception ex)
        {
            //_logger.LogError(ex, $"Error getting meals for {day}/{month}/{year}");
            return StatusCode(500, "Internal server error");
        }
    }

    // GET api/dailymeals/get-monthly-meal/{year}-{month}
    [HttpGet("get-monthly-meal/{year}-{month}")]
    public async Task<ActionResult<IReadOnlyList<DailyMeal>>> GetMonthlyMealsForAllEmployees(int year, int month)
    {
        try
        {
            var meals = await _mealService.MealForMonthAsync(year, month);
            return Ok(meals.ToList());
        }
        catch (Exception ex)
        {
            //_logger.LogError(ex, $"Error getting meals for {month}/{year}");
            return StatusCode(500, "Internal server error");
        }
    }
    [HttpGet("checkMeal/{cardNumber}")]
    public async Task<IActionResult> CheckMeal(string cardNumber)
    {
        if (string.IsNullOrWhiteSpace(cardNumber))
            return BadRequest("Card number is required.");

        var dateTime = DateTime.Now;

        var result = await _mealService.AutoDetectAndCheckMealAsync(cardNumber, dateTime);
        if (result == null)
            return NotFound("User not found or meal time configuration missing.");

        return Ok(result);
    }

    [HttpPost("closeMealRequest")]
    public async Task<IActionResult> CloseMealRequest()
    {
        await _updaterService.RequestManualTrigger();
        return Ok("Meal status update triggered");
    }

    [HttpPost("generateMonthlyMeal/{year}/{month}")]
    public async Task<IActionResult> GenerateMeals(int year, int month)
    {
        await _generator.GenerateMonthlyMealsAsync(year, month);
        return Ok($"Meals generated for {month}/{year}");
    }
}