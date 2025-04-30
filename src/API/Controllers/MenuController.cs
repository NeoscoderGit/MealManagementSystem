using Application.DTOs;
using Application.Interfaces;
using Domain.Entitys.AuthModel;
using Microsoft.AspNetCore.Mvc;

namespace YourNamespace.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MenuController : ControllerBase
    {
        private readonly IMenuService _menuService;

        public MenuController(IMenuService menuService)
        {
            _menuService = menuService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var menus = await _menuService.GetAllAsync();
            var result = menus.Select(m => new MenuDto
            {
                Id = m.Id,
                Name = m.Name,
                Url = m.Url,
                ParentId = m.ParentId
            });
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateMenuDto dto)
        {
            var menu = new Menu
            {
                Name = dto.Name,
                Url = dto.Url,
                ParentId = dto.ParentId
            };

            var createdMenu = await _menuService.CreateAsync(menu);
            return Ok(createdMenu.Id);
        }
    }
}
