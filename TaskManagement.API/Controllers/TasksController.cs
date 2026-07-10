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
        [Route("{PublicId: Guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid Id)
        {
            var result = await _taskService.GetTaskByIdAsync(Id);
            if(result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }
        [HttpPost]
        public async Task<IActionResult> CreateTask([FromBody] AddTaskRequestDTO addTaskRequestDTO)
        {
            var taskDTO = _taskService.CreateTaskAsync(addTaskRequestDTO);
            return CreatedAtAction(nameof(GetById), new {id = taskDTO.Id}, taskDTO);
        }
        [HttpPut("{publicId:guid}")]
        public async Task<IActionResult> UpdateTask([FromBody] UpdateTaskRequestDTO updateTaskRequestDTO, [FromRoute] Guid Id)
        {
            var userDetails = await _userService.GetByPublicIdAsync(updateTaskRequestDTO.AssignToPublicId);
            var result = await _taskService.UpdateTaskAsync(updateTaskRequestDTO, Id, userDetails);
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
