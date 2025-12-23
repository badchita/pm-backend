using System.ComponentModel.DataAnnotations;
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

        [Required]
        [JsonIgnore]
        public string PasswordHash { get; set; } = string.Empty;

        [JsonIgnore]
        public ICollection<TaskCommentReaction> TaskCommentReactions { get; set; }= new List<TaskCommentReaction>();
    }
}
