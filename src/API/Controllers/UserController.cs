using Application.DTOs;
using Application.Interfaces;
using Domain.Entitys.AuthModel;
using Microsoft.AspNetCore.Mvc;


namespace YourNamespace.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var user = await _userService.GetByIdWithRoalAsync(id);
            if (user == null) return NotFound();

            var result = new UserDto
            {
                Id = user.Id,
                EmployeeId = user.EmployeeId,
                EmployeeName = user.EmployeeName,
                EmployeeCode = user.EmployeeCode,
                OfficeEmail = user.OfficeEmail,
                ContactNo = user.ContactNo,
                Roles = user.UserRoles.Select(ur => ur.Role.Name).ToList()
            };
            return Ok(result);
        }
        [HttpGet("getallUser")]
        public async Task<IActionResult> GetallUser()
        {
            var users = await _userService.GetAllAsync();
            if (users == null || !users.Any()) return NotFound();

            var result = users.Select(user => new UserDto
            {
                Id = user.Id,
                EmployeeId = user.EmployeeId,
                EmployeeName = user.EmployeeName,
                EmployeeCode = user.EmployeeCode,
                OfficeEmail = user.OfficeEmail,
                ContactNo = user.ContactNo,
            }).ToList();

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateUserDto dto)
        {
            var user = new User
            {
                EmployeeId = dto.EmployeeId,
                EmployeeName = dto.EmployeeName,
                EmployeeCode = dto.EmployeeCode,
                OfficeEmail = dto.OfficeEmail,
                ContactNo = dto.ContactNo,
                Password =dto.Password
            };

            var createdUser = await _userService.CreateUserAsync(user);

            return Ok(createdUser.Id);
        }
    }
}
