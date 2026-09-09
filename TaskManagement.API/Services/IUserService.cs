using TaskManagement.API.Model.Domain;
using TaskManagement.API.Model.DTO;

namespace TaskManagement.API.Services
{
    public interface IUserService
    {
        Task<UserDTO?> GetByPublicIdAsync(Guid publicId);
        Task<UserDTO> AddUserProfileAsync(UserProfile userProfile);
        Task<UserDTO> GetByIdentityUserIdAsync(string identityUserId);
        Task<IEnumerable<AssignableUsersResponseDto>> GetAssignableUsersAsync();
        Task<IEnumerable<TaskDTO?>> GetMyTasksAsync();
        Task<UpdateUserRoleResponseDto?> UpdateUserRoleAsync(Guid publicId, UpdateUserRoleRequestDto request);
        Task<IEnumerable<UserManagementResponseDto>> GetUserManagementUsersAsync();
    }
}
