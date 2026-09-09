using Microsoft.EntityFrameworkCore;
using TaskManagement.API.Data;
using TaskManagement.API.Model.Domain;
using TaskManagement.API.Model.DTO;
using static TaskManagement.API.Model.Domain.Enum;

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

        public async Task<TaskItem?> DeleteTaskAsync(Guid PublicId)
        {
            var existingTask = await _context.TaskItems
                .Include(u => u.AssignedTo)
                .Include(t => t.CreatedBy)
                .FirstOrDefaultAsync(x => x.PublicId == PublicId);
            if (existingTask == null) { return null; }

            _context.TaskItems.Remove(existingTask);
            await _context.SaveChangesAsync();
            return existingTask;
        }

        public async Task<IEnumerable<TaskItem>> GetAllTasksAsync(string? filterOn = null, TskStatus? filterQuery = null, 
            TaskPriority? priority = null, string? sortBy = null, bool isAscending = true,
            int pageNumber = 1, int pageSize = 1000)
        {
            var records = _context.TaskItems.Include(u => u.AssignedTo)
                .Include(t => t.CreatedBy).AsQueryable();

            //Filtering
            if(string.IsNullOrWhiteSpace(filterOn) == false && (filterQuery != null || priority != null))
            {
                if(filterOn.Equals("Status", StringComparison.OrdinalIgnoreCase))
                {
                    records = records.Where(x => x.Status == filterQuery);
                }
                else if(filterOn.Equals("Priority", StringComparison.OrdinalIgnoreCase))
                {
                    records = records.Where(x => x.Priority == priority);
                }
            }

            //Sorting
            if(string.IsNullOrWhiteSpace(sortBy) == false)
            {
                if(sortBy.Equals("CreatedDate", StringComparison.OrdinalIgnoreCase) )
                {
                    records = isAscending ? records.OrderBy(x => x.CreatedDate) : records.OrderByDescending(x => x.CreatedDate);
                }
                else if(sortBy.Equals("DueDate", StringComparison.OrdinalIgnoreCase))
                {
                    records = isAscending ? records.OrderBy(x => x.DueDate) : records.OrderByDescending(x => x.DueDate);
                }
            }

            //Pagination
            var skipRows = (pageNumber - 1) * pageSize;

            return await records.Skip(skipRows).Take(pageSize).ToListAsync();
        }

        public async Task<TaskItem?> GetTaskByIdAsync(Guid taskId)
        {
            return await _context.TaskItems
                .Include(u => u.AssignedTo)
                .Include(t => t.CreatedBy)
                .FirstOrDefaultAsync(x => x.PublicId == taskId);
        }

        public async Task<TaskItem?> UpdateTaskAsync(UpdateTaskRequestDTO taskItem, Guid publicId, int userId)
        {
            var existingTask = await _context.TaskItems.
                Include(t => t.CreatedBy).FirstOrDefaultAsync(x => x.PublicId == publicId);

            if (existingTask == null) { return null; }

            existingTask.AssignedToId = userId;
            existingTask.Description = (taskItem.Description.Equals("") || taskItem.Description == existingTask.Description) ? existingTask.Description : taskItem.Description;
            existingTask.Status = (taskItem.Status.Equals("") || taskItem.Status == existingTask.Status) ? existingTask.Status : taskItem.Status;
            existingTask.DueDate = (taskItem.DueDate.Equals("") || taskItem.DueDate == existingTask.DueDate) ? existingTask.DueDate : taskItem.DueDate;
            existingTask.Priority = (taskItem.Priority.Equals("") || taskItem.Priority == existingTask.Priority) ? existingTask.Priority : taskItem.Priority;

            await _context.SaveChangesAsync();
            return existingTask;
        }
    }
}
