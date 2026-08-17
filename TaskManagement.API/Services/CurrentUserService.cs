using System.Security.Claims;

namespace TaskManagement.API.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        public string? IdentityUserId =>
         httpContextAccessor.HttpContext?
            .User
            .FindFirstValue(ClaimTypes.NameIdentifier);

        public bool IsAuthenticated =>
             httpContextAccessor.HttpContext?
                .User
                .Identity?
                .IsAuthenticated ?? false;
    }
}
