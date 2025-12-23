using pm_backend.Models;

namespace pm_backend.DTOs
{
    public class TaskCommentReactionRequest
    {
        public int Id { get; set; }
        public int TaskCommentId { get; set; }
        public TaskComment TaskComment { get; set; } = null!;
        public int UserId { get; set; }
        public CommentReactionType? ReactionType { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
