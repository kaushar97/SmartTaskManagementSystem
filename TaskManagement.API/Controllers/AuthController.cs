using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TaskManagement.API.Model.DTO;
using TaskManagement.API.Services;

namespace TaskManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService authService;

        public AuthController(IAuthService authService)
        {
            this.authService = authService;
        }
        // POST: api/Auth/Register
        [HttpPost]
        [Route("Register")]
        public async Task <IActionResult> Register([FromBody] RegisterRequestDto registerRequestDto)
        {
            var result = await authService.RegisterAsync(registerRequestDto);

            return (result)
                ? Ok("User registered successfully with roles.")
                : BadRequest("Something went wrong!!");
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequestDto)
        {
            // Implement login logic here
            var result = await authService.LoginAsync(loginRequestDto);
            return (result != null)
                ? Ok(result)
                : BadRequest("Username or Password is wrong.");
        }
    }
}
