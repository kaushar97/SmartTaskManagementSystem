using TaskManagement.API.Model.DTO;

namespace TaskManagement.API.Services
{
    public interface IUserService
    {
        Task<UserDTO?> GetByPublicIdAsync(Guid publicId);
    }
}
