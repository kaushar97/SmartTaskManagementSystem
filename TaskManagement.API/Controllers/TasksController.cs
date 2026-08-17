using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using TaskManagement.API.Model.Domain;
using TaskManagement.API.Model.DTO;
using TaskManagement.API.Services;
using static TaskManagement.API.Model.Domain.Enum;

namespace TaskManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly ITaskService _taskService;
        private readonly IUserService _userService;

        public TasksController(ITaskService taskService, IUserService userService)
        {
            _taskService = taskService;
            _userService = userService;
        }
        // GET: /api/Tasks?filterOn=Status&filterQuery=InProgress&sortBy=DueDate&isAscending=true&pageNumber=1&pageSize=10
        [HttpGet]
        [Authorize(Roles = "Reader,Writer")]
        public async Task<IActionResult> GetAll([FromQuery] string? filterOn, [FromQuery] TskStatus? filterQuery,
            [FromQuery] TaskPriority? priority, [FromQuery] string? sortBy, [FromQuery] bool? isAscending,
            [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 1000)
        { 
            return Ok(await _taskService.GetAllTasksAsync(filterOn, filterQuery, priority, 
                sortBy, isAscending ?? true, pageNumber, pageSize));
        }

        [HttpGet]
        [Authorize(Roles = "Reader,Writer")]
        [Route("{PublicId:guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid PublicId)
        {
            var result = await _taskService.GetTaskByIdAsync(PublicId);
            if(result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }
        [HttpPost]
        [Authorize(Roles = "Writer, Reader")]
        public async Task<IActionResult> CreateTask([FromBody] AddTaskRequestDTO addTaskRequestDTO)
        {
            var taskDTO = await _taskService.CreateTaskAsync(addTaskRequestDTO);
            if (taskDTO == null)
            {
                return BadRequest(new { message = "Task creation failed. Either the user is logged off or the assigned/created by user could not be found." });
                
            }
            return CreatedAtAction(nameof(GetById), new {PublicId = taskDTO.PublicId}, taskDTO);
        }
        [HttpPut("{PublicId:guid}")]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> UpdateTask([FromBody] UpdateTaskRequestDTO updateTaskRequestDTO, [FromRoute] Guid PublicId)
        {
            var userDetails = await _userService.GetByPublicIdAsync(updateTaskRequestDTO.AssignToPublicId);
            if (userDetails == null)
            {
                return NotFound("AssignToPublicId not found");
            }
            var result = await _taskService.UpdateTaskAsync(updateTaskRequestDTO, PublicId, userDetails);
            if(result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }
        [HttpDelete]
        [Route("{PublicId:guid}")]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> DeleteTask([FromRoute] Guid PublicId)
        {
            var result = await _taskService.DeleteTaskAsync(PublicId);
            if(result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }
    }
}
