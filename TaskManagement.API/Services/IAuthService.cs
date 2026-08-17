using TaskManagement.API.Model.DTO;

namespace TaskManagement.API.Services
{
    public interface IAuthService
    {
        Task<bool> RegisterAsync(RegisterRequestDto registerRequestDto);
        Task<LoginResponseDto?> LoginAsync(LoginRequestDto loginRequestDto);
    }
}
