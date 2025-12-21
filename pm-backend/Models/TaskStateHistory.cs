using pm_backend.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class TaskStateHistory
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int TaskId { get; set; }

    [ForeignKey("TaskId")]
    public ProjectTask Task { get; set; }

    [Required]
    public TaskState PreviousState { get; set; }

    [Required]
    public TaskState NewState { get; set; }

    [Required]
    [MaxLength(255)]
    public string ChangedBy { get; set; }

    [Required]
    public DateTime ChangedAt { get; set; } = DateTime.UtcNow;
}
