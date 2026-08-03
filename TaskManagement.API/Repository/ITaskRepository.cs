using TaskManagement.API.Model.Domain;
using TaskManagement.API.Model.DTO;
using static TaskManagement.API.Model.Domain.Enum;

namespace TaskManagement.API.Repository
{
    public interface ITaskRepository
    {
        Task<IEnumerable<TaskItem>> GetAllTasksAsync(string? filterOn = null, TskStatus? filterQuery = null, 
            TaskPriority? priority = null, string? sortBy = null, bool isAscending = true,
            int pageNumber = 1, int pageSize = 1000);
        Task<TaskItem?> GetTaskByIdAsync(Guid taskId);
        Task<TaskItem> CreateTaskAsync(TaskItem taskItem);
        Task<TaskItem?> UpdateTaskAsync(UpdateTaskRequestDTO taskItem, Guid publicId, int usedId);
        Task<TaskItem?> DeleteTaskAsync(Guid PublicId);
    }
}
