
using Domain.Entitys.AuthModel;

namespace Application.Interfaces
{
    public interface IRoleService
    {
        Task<Role> GetByIdAsync(int id);
        Task<List<Role>> GetAllAsync();
        Task<Role> CreateAsync(Role role);
        Task UpdateAsync(Role role);
        Task DeleteAsync(int id);
    }
}
