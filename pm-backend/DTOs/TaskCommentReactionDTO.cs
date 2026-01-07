using pm_backend.Models;

namespace pm_backend.DTOs
{
    public class TaskCommentReactionRequest
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public CommentReactionType? ReactionType { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
