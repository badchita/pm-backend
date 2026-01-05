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
        public string IsApproved { get; set; } = "";
        public UserRole Role { get; set; }
        public int? CompanyId { get; set; }
        public Company? Company { get; set; }
    }
}
