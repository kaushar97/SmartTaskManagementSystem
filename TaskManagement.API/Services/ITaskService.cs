using TaskManagement.API.Model.Domain;
using TaskManagement.API.Model.DTO;

namespace TaskManagement.API.Services
{
    public interface ITaskService
    {
        Task<IEnumerable<TaskDTO>> GetAllTasksAsync();
        Task<TaskDTO?> GetTaskByIdAsync(Guid Id);
        Task<TaskDTO> CreateTaskAsync(AddTaskRequestDTO addTaskRequestDTO);
        Task<TaskDTO?> UpdateTaskAsync(UpdateTaskRequestDTO updateTaskRequestDTO, Guid publicId, UserDTO userId);
        Task<TaskDTO?> DeleteTaskAsync(int Id);
    }
}
