using static TaskManagement.API.Model.Domain.Enum;

namespace TaskManagement.API.Model.DTO
{
    public class AddTaskRequestDTO
    {
        public required string Title { get; set; }
        public required string Description { get; set; }
        public TskStatus Status { get; set; }
        public TaskPriority Priority { get; set; }
        public DateTime DueDate { get; set; }
        public int AssignedToId { get; set; }
    }
}
