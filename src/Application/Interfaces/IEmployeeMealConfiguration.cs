using Application.DTOs;
using Domain.Entitys.MealModel;

namespace Application.Interfaces
{
    public interface IEmployeeMealConfiguration
    {
        Task<EmployeeMealDayDto> GetByIdAsync(int id);
        Task<List<EmployeeMealDayDto>> GetAllAsync();
        Task<EmployeeMealDayDto> CreateAsync(EmployeeMealDayDto meal);
        Task UpdateAsync(EmployeeMealDayDto meal);
        Task DeleteAsync(int id);
    }
}
