using System.ComponentModel.DataAnnotations;

namespace pm_backend.DTOs
{
    public class CreateTaskCommentRequest
    {
        [Required]
        public string Content { get; set; } = null!;

        [Required]
        public int UserId { get; set; }
    }
}
