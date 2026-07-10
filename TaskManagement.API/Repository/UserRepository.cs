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
        public async Task<User?> GetByPublicIdAsync(Guid guid)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.PublicId == guid);
        }
    }
}
