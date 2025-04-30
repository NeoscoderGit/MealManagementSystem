using Domain.Interfaces.Generic;
using Domain.Interfaces.Meal;
using Infrastructure.Data.Context;
using Infrastructure.Repositories.Generic;
using Infrastructure.Repositories.Meal;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public static class InfrastructureServiceRegistration
    {
        public static IServiceCollection AddInfrastructureService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("MSSqlDB")));
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped(typeof(IDailyMealRepository), typeof(DailyMealRepository));
            return services;
        }
    }
}
