using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace pm_backend.Models
{
    public class Company
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        public string CompanyEmail { get; set; } = string.Empty;

        [Required]
        [Column(TypeName = "char(1)")]
        public string IsApproved { get; set; } = "N";

        [Required]
        [Column(TypeName = "char(1)")]
        public string IsDeleted { get; set; } = "N";

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [JsonIgnore]
        public ICollection<User> Users { get; set; } = new List<User>();

        [JsonIgnore]
        public ICollection<Project> Projects { get; set; } = new List<Project>();

        public ICollection<ProjectTask> ProjectTasks { get; set; } = new List<ProjectTask>();
    }
}
