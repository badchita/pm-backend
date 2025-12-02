using System.ComponentModel.DataAnnotations;

namespace pm_backend.DTOs
{
    public class CreateProjectRequest
    {
        [Required]
        public string ProjectName { get; set; } = null!;

        public string? Description { get; set; }
    }
}
