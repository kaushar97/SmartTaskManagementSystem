using Microsoft.AspNetCore.Mvc;
using TaskManagement.API.Model.Domain;
using TaskManagement.API.Model.DTO;
using static TaskManagement.API.Model.Domain.Enum;

namespace TaskManagement.API.Services
{
    public interface ITaskService
    {
        Task<IEnumerable<TaskDTO>> GetAllTasksAsync(string? filterOn = null, TskStatus? filterQuery = null, 
            TaskPriority? priority = null, string? sortBy = null, bool isAscending = true,
            int pageNumber = 1, int pageSize = 1000);
        Task<TaskDTO?> GetTaskByIdAsync(Guid Id);
        Task<TaskDTO?> CreateTaskAsync(AddTaskRequestDTO addTaskRequestDTO);
        Task<TaskDTO?> UpdateTaskAsync(UpdateTaskRequestDTO updateTaskRequestDTO, Guid publicId, UserDTO userId);
        Task<TaskDTO?> DeleteTaskAsync(Guid PublicId);
    }
}
