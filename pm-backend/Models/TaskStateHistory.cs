using pm_backend.Models;
using System.ComponentModel.DataAnnotations;

public class TaskStateHistory
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int TaskId { get; set; }

    public TaskState? PreviousState { get; set; } = null;

    [Required]
    public TaskState NewState { get; set; }

    [Required]
    [MaxLength(255)]
    public string ChangedBy { get; set; }

    [Required]
    public DateTime ChangedAt { get; set; } = DateTime.UtcNow;
}
