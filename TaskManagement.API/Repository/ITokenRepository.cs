using Microsoft.AspNetCore.Identity;

namespace TaskManagement.API.Repository
{
    public interface ITokenRepository
    {
        string CreateJWTToken(IdentityUser user, List<string> roles);
    }
}
