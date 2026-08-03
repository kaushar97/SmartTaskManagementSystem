namespace TaskManagement.API.Services
{
    public interface ICurrentUserService
    {
        string? IdentityUserId { get; }
        bool IsAuthenticated { get; }
    }
}
