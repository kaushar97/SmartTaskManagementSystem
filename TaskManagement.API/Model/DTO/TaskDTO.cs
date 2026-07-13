using System.ComponentModel.DataAnnotations.Schema;
using TaskManagement.API.Model.Domain;
using static TaskManagement.API.Model.Domain.Enum;

namespace TaskManagement.API.Model.DTO
{
    public class TaskDTO
    {
        public Guid PublicId { get; set; } 
        public required string Title { get; set; }
        public required string Description { get; set; }
        public TskStatus Status { get; set; }
        public TaskPriority Priority { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime DueDate { get; set; }
        public Guid? AssignedToPublicId { get; set; }
    }
}
