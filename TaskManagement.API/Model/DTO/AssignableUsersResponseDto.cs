namespace TaskManagement.API.Model.DTO
{
    public class AssignableUsersResponseDto
    {
        public Guid PublicId { get; set; }
        public string FullName { get; set; } = string.Empty;
    }
}
