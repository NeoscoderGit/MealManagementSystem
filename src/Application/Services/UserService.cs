using Application.Interfaces;
using Domain.Entitys.AuthModel;
using Domain.Interfaces.Generic;
using Microsoft.EntityFrameworkCore;

namespace Application.Services
{
    public class UserService : IUserService
    {
        private readonly IRepository<User> _userRepository;

        public UserService(IRepository<User> userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User> GetByIdAsync(int id)
        {
            return await _userRepository.GetByIdAsync(id);
        }

        public async Task<List<User>> GetAllAsync()
        {
            return await _userRepository.GetAllAsync();
        }

        public async Task<User> CreateUserAsync(User user)
        {
            await _userRepository.AddAsync(user);
            return user;
        }

        public async Task UpdateUserAsync(User user)
        {
            _userRepository.Update(user);
        }

        public async Task DeleteUserAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user != null)
            {
                _userRepository.Delete(user);
            }
        }

        public async Task<User> GetByIdWithRoalAsync(int id)
        {
            return  await _userRepository.GetByIdAsync(
                        id,
                        query => query
                            .Include(u => u.UserRoles)!
                            .ThenInclude(ur => ur.Role)
                            .ThenInclude(r => r.RoleMenus)
                    );
        }
    }
}
