
using Domain.Entitys.AuthModel;

namespace Application.Interfaces
{
    public interface IUserService
    {
        Task<User> GetByIdAsync(int id);
        Task<User> GetByIdWithRoalAsync(int id);
        Task<List<User>> GetAllAsync();
        Task<User> CreateUserAsync(User user);
        Task UpdateUserAsync(User user);
        Task DeleteUserAsync(int id);
    }
}
