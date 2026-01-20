using pm_backend.DTOs;
using pm_backend.Models;

namespace pm_backend.Services.Commands.Contracts
{
    public interface IAuthService
    {
        Task<User> RegisterUserAsync(DTOs.RegisterRequest registerForm);
        Task<LoginResponse> LoginAsync(LoginRequest loginForm);
        Task<RefreshTokenResponse> RefreshTokenAsync(string refreshToken);
        Task LogoutAsync(string userId);
    }
}
