using Microsoft.EntityFrameworkCore;
using TaskManagement.API.Model.Domain;

namespace TaskManagement.API.Data
{
    public class TaskManagementDbContext: DbContext
    {
        public TaskManagementDbContext(DbContextOptions<TaskManagementDbContext> _dbContextOptions): base(_dbContextOptions)
        {
                
        }
        public DbSet<User> Users { get; set; }
        public DbSet<TaskItem> TaskItems { get; set; }
    }
}
