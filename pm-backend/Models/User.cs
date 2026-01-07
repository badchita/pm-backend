using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace pm_backend.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;

        public int? CompanyId { get; set; }

        public Company? Company { get; set; }

        [Required]
        [JsonIgnore]
        public string PasswordHash { get; set; } = string.Empty;

        [Required]
        public UserRole Role { get; set; } = UserRole.Member;

        [Required]
        [Column(TypeName = "char(1)")]
        public string IsApproved { get; set; } = "N";

        [Required]
        [Column(TypeName = "char(1)")]
        public string IsDeleted { get; set; } = "N";

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [JsonIgnore]
        public ICollection<TaskCommentReaction> TaskCommentReactions { get; set; }= new List<TaskCommentReaction>();
    }

    public enum UserRole
    {
        Member,
        Manager,
        Admin
    }
}
