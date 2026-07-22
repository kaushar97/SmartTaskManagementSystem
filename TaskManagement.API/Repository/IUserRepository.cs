using TaskManagement.API.Model.Domain;

namespace TaskManagement.API.Repository
{
    public interface IUserRepository
    {
        Task<UserProfile?> GetByPublicIdAsync(Guid guid);
        Task<UserProfile> AddUserProfileAsync(UserProfile userProfile);
    }
}
