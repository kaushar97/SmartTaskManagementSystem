using Microsoft.EntityFrameworkCore;
using TaskManagement.API.Data;
using TaskManagement.API.Model.Domain;
using TaskManagement.API.Model.DTO;

namespace TaskManagement.API.Repository
{
    public class TaskRepository : ITaskRepository
    {
        private readonly TaskManagementDbContext _context;
        public TaskRepository(TaskManagementDbContext dbContext)
        {
            _context = dbContext;
        }

        public async Task<TaskItem> CreateTaskAsync(TaskItem taskItem)
        {
            await _context.TaskItems.AddAsync(taskItem);
            await _context.SaveChangesAsync();
            return taskItem;
        }

        public async Task<TaskItem?> DeleteTaskAsync(int taskId)
        {
            var existingTask = await _context.TaskItems.FirstOrDefaultAsync(x => x.Id == taskId);
            if (existingTask == null) { return null; }

            _context.TaskItems.Remove(existingTask);
            await _context.SaveChangesAsync();
            return existingTask;
        }

        public async Task<IEnumerable<TaskItem>> GetAllTasksAsync()
        {
            return await _context.TaskItems.Include(u => u.AssignedTo).ToListAsync();
        }

        public async Task<TaskItem?> GetTaskByIdAsync(Guid taskId)
        {
            return await _context.TaskItems
                .Include(u => u.AssignedTo)
                .FirstOrDefaultAsync(x => x.PublicId == taskId);
        }

        public async Task<TaskItem?> UpdateTaskAsync(UpdateTaskRequestDTO taskItem, Guid publicId, int userId)
        {
            var existingTask = await _context.TaskItems.FirstOrDefaultAsync(x => x.PublicId == publicId);

            if (existingTask == null) { return null; }

            existingTask.AssignedToId = userId;
            existingTask.Status = taskItem.Status;
            existingTask.DueDate = taskItem.DueDate;
            existingTask.Priority = taskItem.Priority;

            await _context.SaveChangesAsync();
            return existingTask;
        }
    }
}
