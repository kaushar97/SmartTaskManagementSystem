using TaskManagement.API.Data;
using TaskManagement.API.Model.Domain;
using Microsoft.EntityFrameworkCore;

namespace TaskManagement.API.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly TaskManagementDbContext _context;
        public UserRepository(TaskManagementDbContext dbContext)
        {
            _context = dbContext;
        }

        public async Task<UserProfile> AddUserProfileAsync(UserProfile userProfile)
        {
            await _context.UsersProfile.AddAsync(userProfile);
            await _context.SaveChangesAsync();
            return userProfile;
        }

        public Task<UserProfile?> GetByIdentityUserIdAsync(string identityUserId)
        {
            return _context.UsersProfile.FirstOrDefaultAsync(u => u.IdentityUserId == identityUserId);
        }

        public async Task<UserProfile?> GetByPublicIdAsync(Guid guid)
        {
            return await _context.UsersProfile.FirstOrDefaultAsync(u => u.PublicId == guid);
        }
    }
}
