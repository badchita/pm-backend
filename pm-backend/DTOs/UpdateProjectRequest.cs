using System;
using System.ComponentModel.DataAnnotations;

namespace pm_backend.DTOs
{
    public class UpdateProjectRequest
    {
        [Required]
        [MaxLength(150)]
        public string ProjectName { get; set; } = null!;

        public string? Description { get; set; }

        public DateTime? DueDate { get; set; }

        [MaxLength(1)]
        public string? IsPublished { get; set; }

        [MaxLength(1)]
        public string? IsDeleted { get; set; }
    }
}
