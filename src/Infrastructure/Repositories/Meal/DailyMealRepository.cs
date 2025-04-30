
using Domain.Entitys.MealModel;
using Domain.Interfaces.Meal;
using Domain.ViewModel;
using Infrastructure.Data.Context;
using Infrastructure.Repositories.Generic;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Meal
{
    // DailyMealRepository.cs
    public class DailyMealRepository : Repository<DailyMeal>, IDailyMealRepository
    {
        private readonly AppDbContext _context;

        public DailyMealRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        // ONLY implement specialized methods
        public async Task<bool> ExistsForEmployeeAsync(int employeeId, int year, int month, int day)
        {
            return await _context.DailyMeals
                .AnyAsync(dm => dm.EmployeeId == employeeId &&
                               dm.Year == year &&
                               dm.Month == month &&
                               dm.Day == day);
        }

        public async Task<DailyMeal> MealForDayByEmployeeAsync(int employeeId, int year, int month, int day)
        {
            return await _context.DailyMeals
                .AsNoTracking()
                .Include(d => d.Employee)
                .FirstOrDefaultAsync(d =>
                    d.EmployeeId == employeeId &&
                    d.Year == year &&
                    d.Month == month &&
                    d.Day == day);
        }

        public async Task<IReadOnlyList<DailyMeal>> MealForMonthByEmployeeAsync(int employeeId, int year, int month)
        {
            return await _context.DailyMeals
                .AsNoTracking()
                .Include(d => d.Employee)
                .Where(d =>
                    d.EmployeeId == employeeId &&
                    d.Year == year &&
                    d.Month == month)
                .OrderBy(d => d.Day)
                .ToListAsync();
        }

        public async Task<IReadOnlyList<DailyMeal>> MealForMonthAsync(int year, int month)
        {
            return await _context.DailyMeals
                .AsNoTracking()
                .Include(d => d.Employee)
                .Where(d =>
                    d.Year == year &&
                    d.Month == month)
                .OrderBy(d => d.EmployeeId)
                .ThenBy(d => d.Day)
                .ToListAsync();
        }

        public async Task<IReadOnlyList<DailyMeal>> MealForDayAsync(int year, int month, int day)
        {
            return await _context.DailyMeals
                .AsNoTracking()
                .Include(d => d.Employee)
                .Where(d =>
                    d.Year == year &&
                    d.Month == month &&
                    d.Day == day)
                .OrderBy(d => d.EmployeeId)
                .ToListAsync();
        }

        public async Task GenerateMonthlyMealsAsync(int year, int month, IEnumerable<int> employeeIds)
        {
            var daysInMonth = DateTime.DaysInMonth(year, month);
            var existingMeals = await _context.DailyMeals
                .Where(dm => dm.Year == year && dm.Month == month)
                .ToListAsync();

            var newMeals = new List<DailyMeal>();

            foreach (var employeeId in employeeIds)
            {
                for (int day = 1; day <= daysInMonth; day++)
                {
                    if (existingMeals.Any(m => m.EmployeeId == employeeId && m.Day == day))
                        continue;

                    newMeals.Add(new DailyMeal
                    {
                        EmployeeId = employeeId,
                        Year = year,
                        Month = month,
                        Day = day
                    });
                }
            }

            if (newMeals.Any())
            {
                await AddRangeAsync(newMeals);// Using inherited method
            }
        }

        public async Task<MealCheckResultViewModel?> AutoDetectAndCheckMealAsync(string cardNumber, DateTime requestDateTime)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.CardNumber == cardNumber);
            if (user == null) return null;

            var mealTime = await _context.MealTimes.FirstOrDefaultAsync();
            if (mealTime == null) return null;

            var date = requestDateTime.Date;
            var time = TimeOnly.FromDateTime(requestDateTime);

            var result=new MealCheckResultViewModel();



            if (time >= mealTime.BreakFastTimeStart && time <= mealTime.BreakFastTimeEnd)
            {
                var dailyMeal = await _context.DailyMeals
                .FirstOrDefaultAsync(m => m.EmployeeId == user.Id && m.Date == date);
                if (dailyMeal == null || dailyMeal.IsBreakfastTaken==true)
                {
                    return new MealCheckResultViewModel
                    {
                        UserName = user.EmployeeName ?? "Unknown",
                        EmployeeID = user.EmployeeId ?? 0,
                        Date = date,
                        Time = time,
                        MealPeriod = "Breakfast",
                        TotalMeal = 0,
                        IsAvailable = false,
                        Message = "You have no meal for breakfast."
                    };
                }

                if (dailyMeal.BreakTotalTaken < (dailyMeal.Breakfast + dailyMeal.BreakfastGuest))
                {
                    result.UserName = user.EmployeeName;
                    result.EmployeeID = user.EmployeeId ??0;
                    result.Date = date;
                    result.Time = time;
                    result.MealPeriod= "Breakfast";
                    result.TotalMeal = Convert.ToDouble(dailyMeal.Breakfast+dailyMeal.BreakfastGuest);
                    result.IsAvailable =true;
                    result.Message = $"You have {(dailyMeal.Breakfast+dailyMeal.BreakfastGuest)-(dailyMeal.BreakTotalTaken+1)} meal rest for breakfast";

                    if((dailyMeal.Breakfast + dailyMeal.BreakfastGuest) == (dailyMeal.BreakTotalTaken + 1))
                    {
                        dailyMeal.BreakTotalTaken = dailyMeal.BreakTotalTaken + 1;
                        dailyMeal.IsBreakfastTaken = true;
                    }
                    else
                    {
                        dailyMeal.BreakTotalTaken = dailyMeal.BreakTotalTaken + 1;
                    }

                    Update(dailyMeal);
                }
            }


            //Lunch 
            else if (time >= mealTime.LunchTimeStart && time <= mealTime.LunchTimeEnd)
            {
                var dailyMeal = await _context.DailyMeals
                .FirstOrDefaultAsync(m => m.EmployeeId == user.Id && m.Date == date);
                if (dailyMeal == null || dailyMeal.IsLunchTaken == true)
                {
                    return new MealCheckResultViewModel
                    {
                        UserName = user.EmployeeName ?? "Unknown",
                        EmployeeID = user.EmployeeId ?? 0,
                        Date = date,
                        Time = time,
                        MealPeriod = "Lunch",
                        TotalMeal = 0,
                        IsAvailable = false,
                        Message = "You have no meal for Lunch."
                    };
                }
                if (dailyMeal.LunchTotalTaken < (dailyMeal.Lunch + dailyMeal.LunchGuest))
                {
                    result.UserName = user.EmployeeName;
                    result.EmployeeID = user.EmployeeId ?? 0;
                    result.Date = date;
                    result.Time = time;
                    result.MealPeriod = "Lunch";
                    result.TotalMeal = Convert.ToDouble(dailyMeal.Lunch + dailyMeal.LunchGuest);
                    result.IsAvailable = true;
                    result.Message = $"You have {(dailyMeal.Lunch + dailyMeal.LunchGuest) - (dailyMeal.LunchTotalTaken + 1)} meal rest for lunch";

                    if ((dailyMeal.Lunch + dailyMeal.LunchGuest) == (dailyMeal.LunchTotalTaken + 1))
                    {
                        dailyMeal.LunchTotalTaken = dailyMeal.LunchTotalTaken + 1;
                        dailyMeal.IsLunchTaken = true;
                    }
                    else
                    {
                        dailyMeal.LunchTotalTaken = dailyMeal.LunchTotalTaken + 1;
                    }
                    Update(dailyMeal);
                }
            }

            //Dinner
            else if (time >= mealTime.DinnerTimeStart && time <= mealTime.DinnerTimeEnd)
            {
                var dailyMeal = await _context.DailyMeals
                .FirstOrDefaultAsync(m => m.EmployeeId == user.Id && m.Date == date);
                if (dailyMeal == null || dailyMeal.IsDinnerTaken == true)
                {
                    return new MealCheckResultViewModel
                    {
                        UserName = user.EmployeeName ?? "Unknown",
                        EmployeeID = user.EmployeeId ?? 0,
                        Date = date,
                        Time = time,
                        MealPeriod = "Dinner",
                        TotalMeal = 0,
                        IsAvailable = false,
                        Message = "You have no meal for Dinner."
                    };
                }

                if (dailyMeal.DinnerTotalTaken < (dailyMeal.Dinner + dailyMeal.DinnerGuest))
                {
                    result.UserName = user.EmployeeName;
                    result.EmployeeID = user.EmployeeId ?? 0;
                    result.Date = date;
                    result.Time = time;
                    result.MealPeriod = "Dinner";
                    result.TotalMeal = Convert.ToDouble(dailyMeal.Dinner + dailyMeal.DinnerGuest);
                    result.IsAvailable = true;
                    result.Message = $"You have {(dailyMeal.Dinner + dailyMeal.DinnerGuest) - (dailyMeal.DinnerTotalTaken + 1)} meal rest for Dinner";

                    if ((dailyMeal.Dinner + dailyMeal.DinnerGuest) == (dailyMeal.DinnerTotalTaken + 1))
                    {
                        dailyMeal.DinnerTotalTaken = dailyMeal.DinnerTotalTaken + 1;
                        dailyMeal.IsDinnerTaken = true;
                    }
                    else
                    {
                        dailyMeal.DinnerTotalTaken = dailyMeal.DinnerTotalTaken + 1;
                    }
                    Update(dailyMeal);
                }
            }
            else
            {
                return new MealCheckResultViewModel
                {
                    UserName = user.EmployeeName ?? "Unknown",
                    EmployeeID = user.EmployeeId??0,
                    Date = date,
                    Time = time,
                    MealPeriod = "Unknown",
                    IsAvailable = false,
                    TotalMeal = 0,
                    Message = "No meal period is active at this time."
                };
            }
            return result;
        }
    }
}
