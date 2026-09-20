using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using TaskManagement.API.Configuration;
using TaskManagement.API.Model.Domain;

namespace TaskManagement.API.Data.Seeder
{
    public class IdentitySeeder
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly TaskManagementDbContext _dbContext;
        private readonly SeedDataOptions _seedDataOptions;

        public IdentitySeeder(
            UserManager<IdentityUser> userManager,
            TaskManagementDbContext dbContext,
            IOptions<SeedDataOptions> seedDataOptions)
        {
            _userManager = userManager;
            _dbContext = dbContext;
            _seedDataOptions = seedDataOptions.Value;
        }

        public async Task SeedAsync()
        {
            await SeedInitialWriterAsync();
        }

        private async Task SeedInitialWriterAsync()
        {
            var writerOptions = _seedDataOptions.InitialWriter;

            if (string.IsNullOrWhiteSpace(writerOptions.Email))
            {
                throw new InvalidOperationException(
                    "Initial writer email is not configured.");
            }

            if (string.IsNullOrWhiteSpace(writerOptions.Password))
            {
                throw new InvalidOperationException(
                    "Initial writer password is not configured.");
            }

            var existingUser = await _userManager.FindByEmailAsync(
                writerOptions.Email);

            if (existingUser != null)
            {
                if (!await _userManager.IsInRoleAsync(
                        existingUser,
                        ApplicationRoles.Writer))
                {
                    var roleResult = await _userManager.AddToRoleAsync(
                        existingUser,
                        ApplicationRoles.Writer);

                    if (!roleResult.Succeeded)
                    {
                        var errors = string.Join(
                            ", ",
                            roleResult.Errors.Select(error => error.Description));

                        throw new InvalidOperationException(
                            $"Failed to assign Writer role to initial writer. Errors: {errors}");
                    }
                }

                return;
            }

            var identityUser = new IdentityUser
            {
                UserName = writerOptions.Email,
                Email = writerOptions.Email,
                EmailConfirmed = true
            };

            var createResult = await _userManager.CreateAsync(
                identityUser,
                writerOptions.Password);

            if (!createResult.Succeeded)
            {
                var errors = string.Join(
                    ", ",
                    createResult.Errors.Select(error => error.Description));

                throw new InvalidOperationException(
                    $"Failed to create initial writer. Errors: {errors}");
            }

            var roleAssignmentResult = await _userManager.AddToRoleAsync(
                identityUser,
                ApplicationRoles.Writer);

            if (!roleAssignmentResult.Succeeded)
            {
                await _userManager.DeleteAsync(identityUser);

                var errors = string.Join(
                    ", ",
                    roleAssignmentResult.Errors.Select(error => error.Description));

                throw new InvalidOperationException(
                    $"Failed to assign Writer role to initial writer. Errors: {errors}");
            }

            var userProfile = new UserProfile
            {
                FirstName = writerOptions.FirstName,
                LastName = writerOptions.LastName,
                IdentityUserId = identityUser.Id,
                Email = writerOptions.Email
            };

            _dbContext.UsersProfile.Add(userProfile);

            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch
            {
                await _userManager.DeleteAsync(identityUser);
                throw;
            }
        }
    }
}
