using System.Text.Json.Serialization;
using TaskManagement.API.Validation;
using static TaskManagement.API.Model.Domain.Enum;

namespace TaskManagement.API.Model.DTO
{
    public class UpdateTaskRequestDTO
    {
        public string? Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public TskStatus Status { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public TaskPriority Priority { get; set; }

        [FutureDateAttribute(ErrorMessage = "Due date must be in the future.")]
        public DateTime DueDate { get; set; }
        public Guid AssignToPublicId { get; set; }
    }
}
