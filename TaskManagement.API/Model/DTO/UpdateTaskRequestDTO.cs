using static TaskManagement.API.Model.Domain.Enum;

namespace TaskManagement.API.Model.DTO
{
    public class UpdateTaskRequestDTO
    {
        public string? Title { get; set; } = string.Empty;
        public TskStatus Status { get; set; }
        public TaskPriority Priority { get; set; }
        public DateTime DueDate { get; set; }
        public Guid AssignToPublicId { get; set; }
    }
}
