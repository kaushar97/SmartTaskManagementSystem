using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskManagement.API.Data;
using TaskManagement.API.Model.Domain;
using TaskManagement.API.Services;
using static TaskManagement.API.Model.Domain.Enum;

namespace TaskManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService userService;

        public UsersController(IUserService userService)
        {
            this.userService = userService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await userService.GetAssignableUsersAsync();
            return Ok(result);
        }

    }
}
