using Application.DTOs;
using Application.Interfaces;
using Application.Services.BackgroundServices;
using Azure;
using Domain.Entitys.MealModel;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/meals")]
public class MealController : ControllerBase
{
    private readonly IMealGeneratorService _generator;
    private readonly MealStatusUpdaterService _updaterService;
    private readonly IMealService _mealService;
    private readonly IEmployeeMealConfiguration _employeeMealConfiguration;

    public MealController(IMealGeneratorService generator, IMealService mealService, IEmployeeMealConfiguration employeeMealConfiguration, MealStatusUpdaterService updaterService)
    {
        _generator = generator;
        _mealService = mealService;
        _updaterService = updaterService;
        _employeeMealConfiguration = employeeMealConfiguration;
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
    [HttpGet("get-monthly-meal-by-employee/{userId}/{year}/{month}")]
    public async Task<ActionResult<IReadOnlyList<DailyMeal>>> GetEmployeeMonthlyMeals(int userId, int year, int month)
    {
        try
        {
            var meals = await _mealService.MealForMonthByEmployeeAsync(userId, year, month);
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
    public async Task<ActionResult<IReadOnlyList<DailyMeal>>> GetDailyMealsForAllEmployees(int year, int month, int day)
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

    [HttpGet("updateDailyMealByProperty/{id}/{propertyName}/{value}")]
    public async Task<IActionResult> UpdateDailyMealByProperty(int id,string propertyName, double value)
    {
        try
        {
            var result = await _mealService.UpdateDailyMealByProperty(id, propertyName, value);
            return Ok(new ApiResponse<bool>(result, success: result, message: "Update Successfully"));
        }
        catch (Exception)
        {
            return Ok(new ApiResponse<bool>(success: false, message: "Not Meal Updated"));
        }
    }

    [HttpGet("UpdateMonthlyMealByProperty/{empId}/{month}/{year}/{propertyName}/{value}")]
    public async Task<IActionResult> UpdateMonthlyMealByProperty(int empId,int month,int year, string propertyName, double value)
    {
        try
        {
            var result = await _mealService.UpdateMonthlyMealByProperty(empId,month,year, propertyName, value);
            return Ok(new ApiResponse<bool>(result, success: result, message: "Update Successfully"));
        }
        catch (Exception)
        {
            return Ok(new ApiResponse<bool>(success: false, message: "Not Meal Updated"));
        }
    }

    [HttpGet("setOffDayMeals/{dayOff}/{month}/{year}/{empId}")]
    public async Task<IActionResult> SetOffDayMeals(string dayOff, int month, int year, int empId)
    {
        try
        {
            var result = await _mealService.SetOffDayMealsAsync(dayOff, month, year, empId);
            return Ok(new ApiResponse<bool>(result, success: result, message: "Update Successfully"));
        }
        catch (Exception)
        {
            return Ok(new ApiResponse<bool>(success: false, message: "Not Meal Updated"));
        }
    }
    [HttpGet("getAllEmployeeConfiguration")]
    public async Task<IActionResult> GetAllEmployeeConfiguration()
    {
        try
        {
            var result = await _employeeMealConfiguration.GetAllAsync();
            return Ok(new ApiResponse<List<EmployeeMealDayDto>>(result, success: true, message: "Get All Successfully"));
        }
        catch (Exception)
        {   
            return Ok(new ApiResponse<List<EmployeeMealDayDto>>(data:null,success: false, message: "Not Meal Updated"));
        }
    }

    [HttpPost("addEmployeeMealConfiguration")]
    public async Task<IActionResult> AddEmployeeMealConfiguration([FromBody] EmployeeMealDayDto mealDay)
    {
        try
        {
            var mealSetup=await _employeeMealConfiguration.CreateAsync(mealDay);
            return Ok(mealSetup);
        }
        catch (Exception)
        {

            throw;
        }
    }

}