using Application.Interfaces;
using Application.Services;
using Application.Services.BackgroundServices;
using Microsoft.Extensions.DependencyInjection;

namespace Application
{
    public static class ApplicationServiceRegistration
    {
        public static IServiceCollection AddApplicationService(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IMenuService, MenuService>();
            services.AddScoped<IMealService, MealService>();
            services.AddScoped<IMealGeneratorService, MealGeneratorService>();
            services.AddHostedService<MonthlyMealWorkerService>();
            services.AddSingleton<MealStatusUpdaterService>();
            services.AddHostedService<MealStatusUpdaterService>();
            return services;
        }
    }
}
