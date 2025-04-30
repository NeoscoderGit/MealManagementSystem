
using Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Application.Services.BackgroundServices
{
     
    public class MonthlyMealWorkerService : BackgroundService
    {
        private readonly IServiceProvider _services;

        public MonthlyMealWorkerService(
            IServiceProvider services)
        {
            _services = services;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var now = DateTime.Now;
                var nextRun = new DateTime(now.Year, now.Month, 1).AddMonths(1);
                var delay = nextRun - now;

                //_logger.LogInformation($"Next run: {nextRun}");
                await Task.Delay(delay, stoppingToken);

                using var scope = _services.CreateScope();
                var generator = scope.ServiceProvider.GetRequiredService<IMealGeneratorService>();
                await generator.GenerateMonthlyMealsAsync(nextRun.Year, nextRun.Month);
            }
        }
    }

}
