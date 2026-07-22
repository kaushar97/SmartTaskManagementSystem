using Microsoft.EntityFrameworkCore;
using TaskManagement.API.Model.Domain;

namespace TaskManagement.API.Data
{
    public class TaskManagementDbContext: DbContext
    {
        public TaskManagementDbContext(DbContextOptions<TaskManagementDbContext> _dbContextOptions): base(_dbContextOptions)
        {
                
        }
        public DbSet<UserProfile> UsersProfile { get; set; }
        public DbSet<TaskItem> TaskItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<TaskItem>()
                .HasOne(t => t.AssignedTo)
                .WithMany(u => u.AssignedTasks)
                .HasForeignKey(t => t.AssignedToId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TaskItem>()
                .HasOne(t => t.CreatedBy)
                .WithMany(u => u.CreatedTasks)
                .HasForeignKey(t => t.CreatedById)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
