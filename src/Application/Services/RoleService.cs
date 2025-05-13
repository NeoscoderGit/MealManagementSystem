
using Application.Interfaces;
using Domain.Entitys.AuthModel;
using Domain.Interfaces.Generic;

namespace Application.Services
{
    public class RoleService : IRoleService
    {
        private readonly IRepository<Role> _roleRepository;

        public RoleService(IRepository<Role> roleRepository)
        {
            _roleRepository = roleRepository;
        }

        public async Task<Role> GetByIdAsync(int id)
        {
            return await _roleRepository.GetByIdAsync(id);
        }

        public async Task<List<Role>> GetAllAsync()
        {
            return await _roleRepository.GetAllAsync();
        }

        public async Task<Role> CreateAsync(Role role)
        {
            await _roleRepository.AddAsync(role);
            return role;
        }

        public async Task UpdateAsync(Role role)
        {
           await _roleRepository.UpdateAsync(role);
        }

        public async Task DeleteAsync(int id)
        {
            var role = await _roleRepository.GetByIdAsync(id);
            if (role != null)
            {
               await _roleRepository.DeleteAsync(role);
            }
        }
    }
}
