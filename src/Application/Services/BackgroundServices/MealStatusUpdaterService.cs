using Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Application.Services.BackgroundServices
{
    public class MealStatusUpdaterService : BackgroundService
    {
        private readonly IServiceProvider _services;
        private readonly ILogger<MealStatusUpdaterService> _logger;
        private readonly SemaphoreSlim _triggerLock = new(1, 1);
        private bool _manualTriggerRequested = false;
        private Timer? _manualTriggerTimer;

        public MealStatusUpdaterService(
            IServiceProvider services,
            ILogger<MealStatusUpdaterService> logger)
        {
            _services = services;
            _logger = logger;
        }

        // Method to call for manual triggering
        public async Task RequestManualTrigger()
        {
            if (_manualTriggerRequested)
                return; // Already pending

            _manualTriggerRequested = true;
            _manualTriggerTimer?.Dispose();

            await _triggerLock.WaitAsync();
            try
            {
                await UpdateMealStatuses();  // Direct call
                _manualTriggerRequested = false;
            }
            finally
            {
                _triggerLock.Release();
            }

            // Auto-reset after 5 seconds if not processed
            _manualTriggerTimer = new Timer(_ =>
            {
                _manualTriggerRequested = false;
            }, null, 5000, Timeout.Infinite);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    // Wait for either:
                    // 1. Next midnight (auto-trigger)
                    // 2. Manual trigger request
                    // 3. Service shutdown
                    var now = DateTime.Now;
                    var nextMidnight = now.Date.AddDays(1);
                    var delay = nextMidnight - now;

                    var delayTask = Task.Delay(delay, stoppingToken);
                    var manualTriggerTask = Task.Run(async () =>
                    {
                        while (!_manualTriggerRequested && !stoppingToken.IsCancellationRequested)
                        {
                            await Task.Delay(100, stoppingToken);
                        }
                    }, stoppingToken);

                    await Task.WhenAny(delayTask, manualTriggerTask);

                    if (stoppingToken.IsCancellationRequested)
                        break;

                    await _triggerLock.WaitAsync(stoppingToken);
                    try
                    {
                        await UpdateMealStatuses();
                        _manualTriggerRequested = false;
                    }
                    finally
                    {
                        _triggerLock.Release();
                    }
                }
                catch (OperationCanceledException)
                {
                    // Service is stopping
                    break;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error in meal status updater");
                    // Prevent tight loop on errors
                    await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
                }
            }
        }

        private async Task UpdateMealStatuses()
        {
            using var scope = _services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            var today = DateTime.Today;
            var mealsToUpdate = await dbContext.DailyMeals
                .Where(m => m.Date == today)
                .ToListAsync();

            foreach (var meal in mealsToUpdate)
            {
                meal.IsBreakfastComplete = true;
                meal.IsLunchComplete = true;
                meal.IsDinnerComplete = true;
            }

            await dbContext.SaveChangesAsync();
            _logger.LogInformation($"Updated meal status for {mealsToUpdate.Count} employees");
        }

        public override void Dispose()
        {
            _manualTriggerTimer?.Dispose();
            _triggerLock.Dispose();
            base.Dispose();
        }
    }
}
