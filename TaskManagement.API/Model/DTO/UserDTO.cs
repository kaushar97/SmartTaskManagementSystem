namespace TaskManagement.API.Model.DTO
{
    public class UserDTO
    {
        public int Id { get; set; }
        public Guid PublicId { get; set; }
        public required string FirstName { get; set; }
        public string? LastName { get; set; }
        public required string Email { get; set; }
    }
}
