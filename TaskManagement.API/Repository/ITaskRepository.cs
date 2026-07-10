using TaskManagement.API.Model.Domain;
using TaskManagement.API.Model.DTO;

namespace TaskManagement.API.Repository
{
    public interface ITaskRepository
    {
        Task<IEnumerable<TaskItem>> GetAllTasksAsync();
        Task<TaskItem?> GetTaskByIdAsync(Guid taskId);
        Task<TaskItem> CreateTaskAsync(TaskItem taskItem);
        Task<TaskItem?> UpdateTaskAsync(UpdateTaskRequestDTO taskItem, Guid publicId, int usedId);
        Task<TaskItem?> DeleteTaskAsync(int taskId);
    }
}
