
using Domain.Entitys.AuthModel;

namespace Application.Interfaces
{
    public interface IMenuService
    {
        Task<Menu> GetByIdAsync(int id);
        Task<List<Menu>> GetAllAsync();
        Task<Menu> CreateAsync(Menu menu);
        Task UpdateAsync(Menu menu);
        Task DeleteAsync(int id);
    }
}
