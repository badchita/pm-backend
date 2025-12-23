namespace pm_backend.Models
{
    public class TaskComment
    {
        public int Id { get; set; }
        public int TaskId { get; set; }
        public ProjectTask Task { get; set; } = null!;
        public int UserId { get; set; }
        public User User { get; set; } = null!;
        public string Content { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public ICollection<TaskCommentReaction> Reactions { get; set; } = new List<TaskCommentReaction>();
    }
}
