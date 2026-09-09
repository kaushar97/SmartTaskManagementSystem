using Microsoft.AspNetCore.Identity;
using TaskManagement.API.Model.Domain;
using TaskManagement.API.Model.DTO;
using TaskManagement.API.Repository;


namespace TaskManagement.API.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly UserManager<IdentityUser> _userManager;

        public UserService(IUserRepository userRepository, ICurrentUserService currentUserService,
            UserManager<IdentityUser> userManager)
        {
            _userRepository = userRepository;
            _currentUserService = currentUserService;
            _userManager = userManager;
        }

        public async Task<UserDTO> AddUserProfileAsync(UserProfile userProfile)
        {
            var userDomainModel = await _userRepository.AddUserProfileAsync(userProfile);

            return new UserDTO
            {
                Id = userDomainModel.Id,
                PublicId = userDomainModel.PublicId,
                FirstName = userDomainModel.FirstName,
                LastName = userDomainModel.LastName,
                Email = userDomainModel.Email,
            };
        }

        public async Task<IEnumerable<AssignableUsersResponseDto>> GetAssignableUsersAsync()
        {
            var usersDomainModel = await _userRepository.GetAllAsync();
            var assignableUsers = usersDomainModel.Select(x => new AssignableUsersResponseDto
            {
                PublicId = x.PublicId,
                FullName = $"{x.FirstName} {x.LastName}"
            });

            return assignableUsers;
        }

        public async Task<UserDTO> GetByIdentityUserIdAsync(string identityUserId)
        {
            var userDomainModel = await _userRepository.GetByIdentityUserIdAsync(identityUserId);
            if (userDomainModel == null) { return null; }
            return new UserDTO
            {
                Id = userDomainModel.Id,
                PublicId = userDomainModel.PublicId,
                FirstName = userDomainModel.FirstName,
                LastName = userDomainModel.LastName,
                Email = userDomainModel.Email,
            };
        }

        public async Task<UserDTO?> GetByPublicIdAsync(Guid publicId)
        {
            var userDomainModel = await _userRepository.GetByPublicIdAsync(publicId);
            if (userDomainModel == null) { return null; };
            return new UserDTO
            {
                Id = userDomainModel.Id,
                PublicId = userDomainModel.PublicId,
                FirstName = userDomainModel.FirstName,
                LastName = userDomainModel.LastName,
                Email = userDomainModel.Email,
            };
        }

        public async Task<IEnumerable<TaskDTO?>> GetMyTasksAsync()
        {
            var getCurrentIdentityUserId = _currentUserService.IdentityUserId;
            var allUsers = await _userRepository.GetAllAsync();
            var getSingleUser = allUsers.FirstOrDefault(u => u.IdentityUserId == getCurrentIdentityUserId);
            var myTasks = await _userRepository.GetMyTasksAsync(getSingleUser.Id);

            if(myTasks == null || !myTasks.Any())
            {
                return Enumerable.Empty<TaskDTO>();
            }

            var myTasksDto = myTasks.Select(x => new TaskDTO
            {
                PublicId = x.PublicId,
                Title = x.Title,
                Description = x.Description,
                Status = x.Status,
                Priority = x.Priority,
                DueDate = x.DueDate,
                CreatedDate = x.CreatedDate,
                AssignedToName = $"{getSingleUser.FirstName} {getSingleUser.LastName}",
                AssignedToPublicId = getSingleUser.PublicId,
            });

            return myTasksDto;
        }

        public async Task<IEnumerable<UserManagementResponseDto>> GetUserManagementUsersAsync()
        {
            var usersDomainModel = await _userRepository.GetAllAsync();
            var usersResponseDto = new List<UserManagementResponseDto>();
            foreach (var profile in usersDomainModel)
            {
                var identityUser = await _userManager.FindByIdAsync(profile.IdentityUserId);

                var roles = await _userManager.GetRolesAsync(identityUser);

                var singleUser = new UserManagementResponseDto
                {
                    PublicId = profile.PublicId,
                    FullName = $"{profile.FirstName} {profile.LastName}",
                    Email = profile.Email,
                    Role = roles[0]
                };

                usersResponseDto.Add(singleUser);
            }
            return usersResponseDto; 
        }

        public async Task<UpdateUserRoleResponseDto?> UpdateUserRoleAsync(Guid publicId, UpdateUserRoleRequestDto request)
        {
            var allowedRoles = new[] { "Reader", "Writer" };

            if (!allowedRoles.Contains(
                    request.Role,
                    StringComparer.OrdinalIgnoreCase))
            {
                throw new ArgumentException(
                    "Role must be either Reader or Writer.");
            }

            var userProfile = await _userRepository.GetByPublicIdAsync(publicId);

            if (userProfile is null)
            {
                return null;
            }

            var identityUser = await _userManager.FindByIdAsync(
                userProfile.IdentityUserId);

            if (identityUser is null)
            {
                return null;
            }

            var currentRoles = await _userManager.GetRolesAsync(identityUser);

            var currentRole = currentRoles.FirstOrDefault();

            if (string.Equals(
                    currentRole,
                    request.Role,
                    StringComparison.OrdinalIgnoreCase))
            {
                return new UpdateUserRoleResponseDto
                {
                    PublicId = userProfile.PublicId,
                    FullName =
                        $"{userProfile.FirstName} {userProfile.LastName}".Trim(),
                    Email = identityUser.Email ?? string.Empty,
                    Role = currentRole
                };
            }

            if (currentRoles.Count > 0)
            {
                var removeResult =
                    await _userManager.RemoveFromRolesAsync(
                        identityUser,
                        currentRoles);

                if (!removeResult.Succeeded)
                {
                    throw new InvalidOperationException(
                        "Failed to remove the user's current role.");
                }
            }

            var newRole = allowedRoles.First(
                role => string.Equals(
                    role,
                    request.Role,
                    StringComparison.OrdinalIgnoreCase));

            var addResult =
                await _userManager.AddToRoleAsync(
                    identityUser,
                    newRole);

            if (!addResult.Succeeded)
            {
                throw new InvalidOperationException(
                    "Failed to assign the new role.");
            }

            return new UpdateUserRoleResponseDto
            {
                PublicId = userProfile.PublicId,
                FullName =
                    $"{userProfile.FirstName} {userProfile.LastName}".Trim(),
                Email = identityUser.Email ?? string.Empty,
                Role = newRole
            };
        }
    }
}
