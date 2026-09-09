using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskManagement.API.Data;
using TaskManagement.API.Model.Domain;
using TaskManagement.API.Model.DTO;
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
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> GetAll()
        {
            var result = await userService.GetAssignableUsersAsync();
            return Ok(result);
        }

        [HttpGet("my")]
        [Authorize(Roles = "Writer, Reader")]
        public async Task<IActionResult> GetMyTasks()
        {
            var result = await userService.GetMyTasksAsync();
            return Ok(result);
        }

        [HttpPatch("{publicId:guid}/role")]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> UpdateUserRole([FromRoute] Guid publicId, [FromBody] UpdateUserRoleRequestDto request)
        {
            var user = await userService.UpdateUserRoleAsync(publicId, request);

            if (user is null)
            {
                return NotFound(new
                {
                    message = "User not found."
                });
            }

            return Ok(user);
        }

        [HttpGet("userManagement")]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> GetAllWithRoles()
        {
            var result = await userService.GetUserManagementUsersAsync();
            if(result is null)
            {
                return NotFound(new { message = "Currently, No users" });
            }
            return Ok(result);
        }

    }
}
