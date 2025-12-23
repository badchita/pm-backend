using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace pm_backend.Models
{
    public class TaskCommentReaction
    {
        public int Id { get; set; }

        [ForeignKey("TaskComment")]
        public int TaskCommentId { get; set; }

        [Required]
        public int UserId { get; set; }
        public User User { get; set; } = null!;

        public CommentReactionType? ReactionType { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }

    public enum CommentReactionType
    {
        Like = 1,
        Dislike = 2
    }
}
