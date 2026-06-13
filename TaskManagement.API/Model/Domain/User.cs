namespace TaskManagement.API.Model.Domain
{
    public class User
    {
        public Guid PublicId { get; set; }
        public int Id { get; set; }
        public required string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public ICollection<TaskItem> Tasks { get; set; }
    }
}
