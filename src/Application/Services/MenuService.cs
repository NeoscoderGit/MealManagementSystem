using Application.Interfaces;
using Domain.Entitys.AuthModel;
using Domain.Interfaces.Generic;

namespace Application.Services
{
    public class MenuService : IMenuService
    {
        private readonly IRepository<Menu> _menuRepository;

        public MenuService(IRepository<Menu> menuRepository)
        {
            _menuRepository = menuRepository;
        }

        public async Task<Menu> GetByIdAsync(int id)
        {
            return await _menuRepository.GetByIdAsync(id);
        }

        public async Task<List<Menu>> GetAllAsync()
        {
            return await _menuRepository.GetAllAsync();
        }

        public async Task<Menu> CreateAsync(Menu menu)
        {
            await _menuRepository.AddAsync(menu);
            return menu;
        }

        public async Task UpdateAsync(Menu menu)
        {
           await _menuRepository.UpdateAsync(menu);
        }

        public async Task DeleteAsync(int id)
        {
            var menu = await _menuRepository.GetByIdAsync(id);
            if (menu != null)
            {
               await _menuRepository.DeleteAsync(menu);
            }
        }
    }
}
