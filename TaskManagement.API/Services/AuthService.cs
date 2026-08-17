using Microsoft.AspNetCore.Identity;
using TaskManagement.API.Model.Domain;
using TaskManagement.API.Model.DTO;
using TaskManagement.API.Repository;

namespace TaskManagement.API.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly IUserService userService;
        private readonly ITokenRepository tokenRepository;

        public AuthService(UserManager<IdentityUser> userManager, IUserService userService, ITokenRepository tokenRepository)
        {
            this.userManager = userManager;
            this.userService = userService;
            this.tokenRepository = tokenRepository;
        }
        public async Task<bool> RegisterAsync(RegisterRequestDto registerRequestDto)
        {
            var identityUser = new IdentityUser
            {
                UserName = registerRequestDto.Username,
                Email = registerRequestDto.Username
            };
            var identityResult = await userManager.CreateAsync(identityUser, registerRequestDto.Password);

            if (identityResult.Succeeded)
            {
                //Add roles to the user
                if (registerRequestDto.Roles != null && registerRequestDto.Roles.Any())
                {
                    identityResult = await userManager.AddToRolesAsync(identityUser, registerRequestDto.Roles);
                    if (identityResult.Succeeded)
                    {
                        var userProfile = new UserProfile
                        {
                            FirstName = registerRequestDto.FirstName,
                            LastName = registerRequestDto.LastName,
                            IdentityUserId = identityUser.Id,
                            Email = registerRequestDto.Username
                        };
                        var userProfileCreated = await userService.AddUserProfileAsync(userProfile);
                        if(userProfileCreated != null)
                        {
                            return true;
                        }
                        else
                        {
                            await userManager.DeleteAsync(identityUser);
                            throw new Exception("Failed to create user profile.");
                        }
                    }
                }

            }
            return false;
        }

        public async Task<LoginResponseDto?> LoginAsync(LoginRequestDto loginRequestDto)
        {
            var user = await userManager.FindByEmailAsync(loginRequestDto.Username);
            if (user != null && await userManager.CheckPasswordAsync(user, loginRequestDto.Password))
            {
                var roles = await userManager.GetRolesAsync(user);

                if(roles != null)
                {
                        // token generation logic here
                        var jwtToken = tokenRepository.CreateJWTToken(user, roles.ToList());
                        var response = new LoginResponseDto
                        {
                            JwtToken = jwtToken
                        };
                    return response;
                }
                
            }
            return null;
        }
    }
}
