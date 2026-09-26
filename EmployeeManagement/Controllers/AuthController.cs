using EmployeeManagement.API.Filters;
using EmployeeManagement.Application.Contracts.Services;
using EmployeeManagement.Application.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }
        [HttpPost("register")]
        [ServiceFilter(typeof(ValidationFilter<RegisterDTO>))]

        public async Task<IActionResult> Register(RegisterDTO dto)
        {
             await _authService.RegisterAsync(dto);
            return StatusCode(201);
        }
        [HttpPost("login")]
        [ServiceFilter(typeof(ValidationFilter<LoginRequest>))]

        public async Task<ActionResult<LoginResponse>> Login(LoginRequest request)
        {
        var response= await _authService.LoginAsync(request);
            return Ok(response);
        }
    }
}
