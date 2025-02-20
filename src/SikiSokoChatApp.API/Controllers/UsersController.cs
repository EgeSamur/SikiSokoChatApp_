using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SikiSokoChatApp.Application.Abstractions.Services;
using SikiSokoChatApp.Application.Features.Users.DTOs;

namespace SikiSokoChatApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _service;

        public UsersController(IUserService userService)
        {
            _service = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync(RegisterDto dto)
        {
            var result = await _service.CreateAsync(dto);
            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync(LoginDto dto)
        {
            var result = await _service.LoginAsync(dto);
            return Ok(result);
        }

        [HttpPost("get-all-users")]
        public async Task<IActionResult> GetAllUsersAsync()
        {
            var result = await _service.GetAllUsers();
            return Ok(result);
        }


        [HttpPost("delete-user")]
        public async Task<IActionResult> DeleteUser(DeleteUserDto dto)
        {
            var result = await _service.DeleteUserAsync(dto);
            return Ok(result);
        }

    }
}
