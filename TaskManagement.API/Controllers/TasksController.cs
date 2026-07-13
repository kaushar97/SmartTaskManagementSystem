using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskManagement.API.Model.Domain;
using TaskManagement.API.Model.DTO;
using TaskManagement.API.Services;

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
        [HttpGet]
        public async Task<IActionResult> GetAll() 
        { 
            return Ok(await _taskService.GetAllTasksAsync());
        }

        [HttpGet]
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
        public async Task<IActionResult> CreateTask([FromBody] AddTaskRequestDTO addTaskRequestDTO)
        {
            var taskDTO = await _taskService.CreateTaskAsync(addTaskRequestDTO);
            return CreatedAtAction(nameof(GetById), new {PublicId = taskDTO.PublicId}, taskDTO);
        }
        [HttpPut("{PublicId:guid}")] 
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
        [Route("{Id:int}")]
        public async Task<IActionResult> DeleteTask([FromRoute] int Id)
        {
            var result = await _taskService.DeleteTaskAsync(Id);
            if(result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }
    }
}
