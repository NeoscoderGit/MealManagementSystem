using Application.DTOs;
using Application.Interfaces;
using Domain.Entitys.AuthModel;
using Microsoft.AspNetCore.Mvc;

namespace YourNamespace.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var roles = await _roleService.GetAllAsync();
            var result = roles.Select(r => new RoleDto { Id = r.Id, Name = r.Name });
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateRoleDto dto)
        {
            var role = new Role { Name = dto.Name };
            var createdRole = await _roleService.CreateAsync(role);
            return Ok(createdRole.Id);
        }
    }
}
