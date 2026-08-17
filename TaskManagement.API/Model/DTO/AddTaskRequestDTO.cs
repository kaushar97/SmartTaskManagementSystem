using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using TaskManagement.API.Validation;
using static TaskManagement.API.Model.Domain.Enum;

namespace TaskManagement.API.Model.DTO
{
    public class AddTaskRequestDTO
    {
        [Required(ErrorMessage = "Title is required.")]
        [StringLength(
        100,
        MinimumLength = 3,
        ErrorMessage = "Title must be between 3 and 100 characters.")]
        public required string Title { get; set; }

        [Required(ErrorMessage = "Description is required.")]
        [StringLength(
        3000,
        ErrorMessage = "Description cannot exceed 3000 characters.")]
        public required string Description { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public TskStatus Status { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public TaskPriority Priority { get; set; }

        [FutureDateAttribute(ErrorMessage = "Due date must be in the future.")]
        public DateTime DueDate { get; set; }
        [Required(ErrorMessage = "Assigned user is required.")]
        public Guid AssignedToPublicId { get; set; }
    }
}
