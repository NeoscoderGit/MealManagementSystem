
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
                    d.Day == day) ?? new DailyMeal();
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

        public async Task<MealCheckResultViewModel> AutoDetectAndCheckMealAsync(string cardNumber, DateTime requestDateTime)
        {
            var result = new MealCheckResultViewModel();
            var user = await _context.Users.FirstOrDefaultAsync(u => u.CardNumber == cardNumber);
            if (user == null)
            {
                return new MealCheckResultViewModel
                {
                    UserName = user?.EmployeeName ?? "Unknown",
                    EmployeeID = user?.EmployeeId ?? 0,
                    Date = requestDateTime.Date,
                    Time = requestDateTime.ToString("hh:mm tt"),
                    MealPeriod = "Breakfast",
                    TotalMeal = 0,
                    IsAvailable = false,
                    Message = "User Not Found."
                };
            }

            var mealTime = await _context.MealTimes.FirstOrDefaultAsync();
            if (mealTime == null)
            {
                return new MealCheckResultViewModel
                {
                    UserName = user.EmployeeName ?? "Unknown",
                    EmployeeID = user.EmployeeId ?? 0,
                    Date = requestDateTime.Date,
                    Time = requestDateTime.ToString("hh:mm tt"),
                    MealPeriod = "Breakfast",
                    TotalMeal = 0,
                    IsAvailable = false,
                    Message = "There is no meal Period."
                };
            }

            var date = requestDateTime.Date;
            var time = TimeOnly.FromDateTime(requestDateTime);

            // Breakfast
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
                        Time = time.ToString("hh:mm tt"),
                        MealPeriod = "Breakfast",
                        TotalMeal = 0,
                        IsAvailable = false,
                        Message = "You have no meal for breakfast."
                    };
                }

                if (dailyMeal.BreakTotalTaken < (dailyMeal.Breakfast + dailyMeal.BreakfastGuest))
                {
                    result.UserName = user.EmployeeName?? "Unknown";
                    result.EmployeeID = user.EmployeeId ??0;
                    result.Date = date;
                    result.Time = time.ToString("hh:mm tt");
                    result.MealPeriod= "Breakfast";
                    result.TotalMeal = Convert.ToDouble(dailyMeal.Breakfast+dailyMeal.BreakfastGuest);
                    result.IsAvailable =true;
                    result.Message = $"You have {(dailyMeal.Breakfast+dailyMeal.BreakfastGuest)-(dailyMeal.BreakTotalTaken+1)} meal remain for breakfast";

                    if((dailyMeal.Breakfast + dailyMeal.BreakfastGuest) == (dailyMeal.BreakTotalTaken + 1))
                    {
                        dailyMeal.BreakTotalTaken = dailyMeal.BreakTotalTaken + 1;
                        dailyMeal.IsBreakfastTaken = true;
                    }
                    else
                    {
                        dailyMeal.BreakTotalTaken = dailyMeal.BreakTotalTaken + 1;
                    }
                     await UpdateAsync(dailyMeal);
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
                        Time = time.ToString("hh:mm tt"),
                        MealPeriod = "Lunch",
                        TotalMeal = 0,
                        IsAvailable = false,
                        Message = "You have no meal for Lunch."
                    };
                }
                if (dailyMeal.LunchTotalTaken < (dailyMeal.Lunch + dailyMeal.LunchGuest))
                {
                    result.UserName = user.EmployeeName ?? "Unknown";
                    result.EmployeeID = user.EmployeeId ?? 0;
                    result.Date = date;
                    result.Time = time.ToString("hh:mm tt");
                    result.MealPeriod = "Lunch";
                    result.TotalMeal = Convert.ToDouble(dailyMeal.Lunch + dailyMeal.LunchGuest);
                    result.IsAvailable = true;
                    result.Message = $"You have {(dailyMeal.Lunch + dailyMeal.LunchGuest) - (dailyMeal.LunchTotalTaken + 1)} meal remain for lunch";

                    if ((dailyMeal.Lunch + dailyMeal.LunchGuest) == (dailyMeal.LunchTotalTaken + 1))
                    {
                        dailyMeal.LunchTotalTaken = dailyMeal.LunchTotalTaken + 1;
                        dailyMeal.IsLunchTaken = true;
                    }
                    else
                    {
                        dailyMeal.LunchTotalTaken = dailyMeal.LunchTotalTaken + 1;
                    }
                    await UpdateAsync(dailyMeal);
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
                        Time = time.ToString("hh:mm tt"),
                        MealPeriod = "Dinner",
                        TotalMeal = 0,
                        IsAvailable = false,
                        Message = "You have no meal for Dinner."
                    };
                }

                if (dailyMeal.DinnerTotalTaken < (dailyMeal.Dinner + dailyMeal.DinnerGuest))
                {
                    result.UserName = user.EmployeeName ?? "Unknown";
                    result.EmployeeID = user.EmployeeId ?? 0;
                    result.Date = date;
                    result.Time = time.ToString("hh:mm tt");
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
                    await UpdateAsync(dailyMeal);
                }
            }
            else
            {
                return new MealCheckResultViewModel
                {
                    UserName = user.EmployeeName ?? "Unknown",
                    EmployeeID = user.EmployeeId??0,
                    Date = date,
                    Time = time.ToString("hh:mm tt"),
                    MealPeriod = "Unknown",
                    IsAvailable = false,
                    TotalMeal = 0,
                    Message = "No meal period is active at this time."
                };
            }
            return result;
        }

        public async Task<bool> UpdateDailyMealByProperty(int id, string propertyName, double value)
        {
            // Fetch the DailyMeal object
            var dailyMeal = await _context.DailyMeals.FirstOrDefaultAsync(m => m.Id == id);
            if (dailyMeal == null)
            {
                throw new ArgumentException($"DailyMeal with ID '{id}' does not exist.");
            }

            // Get the type and property
            var type = dailyMeal.GetType();
            var property = type.GetProperty(propertyName);

            if (property == null)
            {
                throw new ArgumentException($"Property '{propertyName}' does not exist on type '{type.Name}'.");
            }

            if (!property.CanWrite)
            {
                throw new InvalidOperationException($"Property '{propertyName}' is read-only.");
            }

            // Convert and set the value
            try
            {
                object? convertedValue = value;
                // Handle nullable types explicitly
                if (property.PropertyType.IsGenericType && property.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    var underlyingType = Nullable.GetUnderlyingType(property.PropertyType);
                    convertedValue = Convert.ChangeType(value, underlyingType);
                }
                else
                {
                    convertedValue = Convert.ChangeType(value, property.PropertyType);
                }

                property.SetValue(dailyMeal, convertedValue);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to set property '{propertyName}' with value '{value}'.", ex);
            }

            // Persist changes
            await UpdateAsync(dailyMeal);
            return true;
        }

        public async Task<bool> UpdateMonthlyMealByProperty(int empId, int month, int year, string propertyName, double value)
        {
            try
            {
                // Safely allow only specific column names
                string mealType = "";
                var allowedProperties = new[] { "Breakfast", "Lunch", "Dinner", "BreakfastGuest", "LunchGuest", "DinnerGuest" };
                var allowedPropertiesIsAllow = new[] { "IsLunchComplete", "IsBreakfastComplete", "IsDinnerComplete" };
                if (!allowedProperties.Contains(propertyName))
                    throw new ArgumentException("Invalid property name");

                if (propertyName == "Breakfast" || propertyName == "BreakfastGuest" )
                {
                    mealType = "IsBreakfastComplete";
                }
                else if (propertyName == "Lunch" || propertyName == "LunchGuest")
                {
                    mealType = "IsLunchComplete";
                }
                else if (propertyName == "Dinner" || propertyName == "DinnerGuest")
                {
                    mealType = "IsDinnerComplete";
                }

                // Safe SQL with positional parameter placeholders
                var sql = $"UPDATE DailyMeals SET {propertyName} = @p0 WHERE EmployeeId = @p1 AND Month = @p2 AND Year = @p3 AND {mealType} =0";

                var result = await _context.Database.ExecuteSqlRawAsync(sql, value, empId, month, year);

                return result > 0;
            }
            catch (Exception)
            {
               return false;    
            }
            
        }

        public async Task<bool> SetOffDayMealsAsync(string weekend, int month, int year, int employeeId)
        {
            List<int> dayNumbers=new List<int>();
            dayNumbers = GetDayOff(weekend, month, year);
            if (dayNumbers == null || !dayNumbers.Any())
                return false;

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var dayParams = string.Join(",", dayNumbers.Select((d, i) => $"@p{i + 3}"));

                var sql = $@"
                            UPDATE DailyMeals
                            SET Breakfast = 0,
                                BreakfastGuest = 0,
                                Lunch = 0,
                                LunchGuest = 0,
                                Dinner = 0,
                                DinnerGuest = 0
                            WHERE EmployeeId = @p0
                                AND Month = @p1
                                AND Year = @p2
                                AND IsBreakfastComplete = 0
                                AND IsLunchComplete = 0
                                AND IsDinnerComplete = 0
                                AND Day IN ({dayParams});
                        ";

                var parameters = new List<object> { employeeId, month, year };
                parameters.AddRange(dayNumbers.Cast<object>());

                var result = await _context.Database.ExecuteSqlRawAsync(sql, parameters.ToArray());

                await transaction.CommitAsync();

                return result > 0;
            }
            catch
            {
                await transaction.RollbackAsync();
                return false;
            }
        }

        static List<int> GetDayOff(string dayName, int month, int year)
        {
            //DateTime m = DateTime.Now;
            //int month = m.Month;
            //var year = m.Year;
            if (!Enum.TryParse(typeof(DayOfWeek), dayName, true, out var dayOfWeekObj))
                throw new ArgumentException("Invalid day name");

            var dayOfWeek = (DayOfWeek)dayOfWeekObj;

            int daysInMonth = DateTime.DaysInMonth(year, month);
            var dayNumbers = new List<int>();

            for (int day = 1; day <= daysInMonth; day++)
            {
                var date = new DateTime(year, month, day);
                if (date.DayOfWeek == dayOfWeek)
                {
                    dayNumbers.Add(day);
                }
            }

            return dayNumbers;
        }

        //    public async Task<int> UpdateColumnAsync<T>(
        //Expression<Func<DailyMeal, bool>> whereClause,
        //string propertyName,
        //T value)
        //    {
        //        // Fetch the rows matching the where clause
        //        var dailyMeals = await _context.DailyMeals
        //            .Where(whereClause)
        //            .ToListAsync();

        //        if (!dailyMeals.Any())
        //        {
        //            return 0; // No rows to update
        //        }

        //        // Get the property to update using reflection
        //        var property = typeof(DailyMeal).GetProperty(propertyName);
        //        if (property == null || !property.CanWrite)
        //        {
        //            throw new ArgumentException($"Property '{propertyName}' does not exist or is not writable.");
        //        }

        //        // Update the property value for each row
        //        foreach (var dailyMeal in dailyMeals)
        //        {
        //            property.SetValue(dailyMeal, value);
        //        }

        //        // Save changes to the database
        //        return await _context.SaveChangesAsync();
        //    }
    }
}
