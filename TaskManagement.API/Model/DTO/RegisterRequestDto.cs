using System.ComponentModel.DataAnnotations;

namespace TaskManagement.API.Model.DTO
{
    public class RegisterRequestDto
    {
        public required string FirstName { get; set; }
        public string? LastName { get; set; }

        [DataType(DataType.EmailAddress)]
        public required string Username { get; set; }

        [DataType(DataType.Password)]
        public required string Password { get; set; }
        public string[]? Roles { get; set; }
    }
}
