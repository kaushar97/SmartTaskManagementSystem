namespace TaskManagement.API.Configuration
{
    public class SeedDataOptions
    {
        public InitialWriterOptions InitialWriter { get; set; } = new();
    }
    public class InitialWriterOptions
    {
        public string FirstName { get; set; } = string.Empty;
        public string? LastName { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
