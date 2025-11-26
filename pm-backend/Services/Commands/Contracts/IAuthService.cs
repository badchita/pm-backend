using pm_backend.DTOs;
using pm_backend.Models;

namespace pm_backend.Services.Commands.Contracts
{
    public interface IAuthService
    {
        Task<User> RegisterUser(DTOs.RegisterRequest registerForm);
        Task<LoginResponse> Login(LoginRequest loginForm);
    }
}
