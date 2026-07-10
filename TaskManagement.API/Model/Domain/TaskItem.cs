using System.ComponentModel.DataAnnotations.Schema;
using static TaskManagement.API.Model.Domain.Enum;

namespace TaskManagement.API.Model.Domain
{
    public class TaskItem
    {
        public int Id { get; set; }
        public Guid PublicId { get; set; } = Guid.NewGuid();
        public required string Title { get; set; }
        public required string Description { get; set; }
        public TskStatus Status { get; set; }
        public TaskPriority Priority { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime DueDate { get; set; }
        public int AssignedToId { get; set; }

        //Navigation property
        [ForeignKey("AssignedToId")]
        public User AssignedTo { get; set; } = null!;
    }
}
