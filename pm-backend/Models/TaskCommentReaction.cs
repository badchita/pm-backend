using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace pm_backend.Models
{
    public class TaskCommentReaction
    {
        public int Id { get; set; }

        [ForeignKey("TaskComment")]
        public int TaskCommentId { get; set; }
        public TaskComment TaskComment { get; set; } = null!;

        [Required]
        public int UserId { get; set; }

        public CommentReactionType ReactionType { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }

    public enum CommentReactionType
    {
        Like = 1,
        Dislike = 2
    }
}
