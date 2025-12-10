using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace pm_backend.Models
{
    public class ProjectTask
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(255)]
        public string TaskName { get; set; } = string.Empty;

        public string? Description { get; set; }
        public string? AcceptanceCriteria { get; set; }

        [MaxLength(255)]
        public string? AssignedTo { get; set; }

        public int? TaskPoints { get; set; }

        [MaxLength(50)]
        public string TaskIdNumber { get; set; } = string.Empty;

        public TaskState State { get; set; } = TaskState.New;

        public DateTime? ReadyForDevelopmentDate { get; set; }
        public DateTime? DoneDate { get; set; }
        public DateTime? TestingStartDate { get; set; }
        public DateTime? TestingEndDate { get; set; }

        [MaxLength(255)]
        public string CreatedBy { get; set; } = string.Empty;

        [MaxLength(255)]
        public string? UpdatedBy { get; set; }

        [ForeignKey("Project")]
        public int ProjectId { get; set; }

        [JsonIgnore]
        public Project Project { get; set; }
    }

    public enum TaskState
    {
        New,
        Refinement,
        ReadyForDevelopment,
        InProgress,
        Testing,
        Deployed,
        Closed
    }
}
