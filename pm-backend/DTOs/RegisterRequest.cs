using pm_backend.Models;

namespace pm_backend.DTOs
{
    public class RegisterRequest
    {
        public string Name { get; set; } = "";
        public string Email { get; set; } = "";
        public string Password { get; set; } = "";
        public string? CompanyName { get; set; }
        public UserRole Role{ get; set; }
    }
}
