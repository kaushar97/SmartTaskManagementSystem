using TaskManagement.API.Model.Domain;

namespace TaskManagement.API.Repository
{
    public interface IUserRepository
    {
        Task <User?> GetByPublicIdAsync(Guid guid);
    }
}
