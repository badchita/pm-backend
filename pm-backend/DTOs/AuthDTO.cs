using pm_backend.Models;

namespace pm_backend.DTOs
{
    public class LoginRequest
    {
        public string Email { get; set; } = "";

        public string Password { get; set; } = "";
    }

    public class LoginResponse
    {
        public string Token { get; set; } = "";

        public UserDto User { get; set; } = new();

        public string RefreshToken{ get; set; } = "";
    }

    public class RegisterRequest
    {
        public string Name { get; set; } = "";

        public string Email { get; set; } = "";

        public string Password { get; set; } = "";

        public string? CompanyName { get; set; }

        public UserRole Role { get; set; }
    }

    public class RefreshTokenRequest
    {
        public string RefreshToken { get; set; } = null!;
    }

    public class RefreshTokenResponse
    {
        public string Token { get; set; } = "";

        public string RefreshToken { get; set; } = "";
    }
}
