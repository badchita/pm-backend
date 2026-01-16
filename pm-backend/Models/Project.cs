using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace pm_backend.Models
{
    public class Project
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string ProjectIdNumber { get; set; } = null!;

        [Required]
        [MaxLength(150)]
        public string ProjectName { get; set; } = null!;

        public string? Description { get; set; }

        public int CompanyId { get; set; }

        public Company Company { get; set; } = null!;

        [Required]
        [MaxLength(255)]
        public string CreatedBy { get; set; } = null!;

        [Required]
        [Column(TypeName = "char(1)")]
        public string IsPublished { get; set; } = "N";

        [Required]
        [Column(TypeName = "char(1)")]
        public string IsDeleted { get; set; } = "N";

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? DueDate { get; set; }

        [JsonIgnore]
        public ICollection<ProjectTask> Tasks { get; set; } = new List<ProjectTask>();
    }
}
