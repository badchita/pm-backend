using pm_backend.Models;

namespace pm_backend.DTOs
{
    public class LoginResponse
    {
        public string Token { get; set; } = "";
        public UserDto User { get; set; } = new();
    }

    public class UserDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Email { get; set; } = "";
    }
}
