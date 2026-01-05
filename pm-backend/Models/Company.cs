using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<User> Users { get; set; } = new List<User>();
        public ICollection<Project> Projects { get; set; } = new List<Project>();
        public ICollection<ProjectTask> Tasks => Projects.SelectMany(p => p.Tasks).ToList();
    }
}
