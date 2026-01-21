using System.ComponentModel.DataAnnotations;

namespace pm_backend.DTOs
{
    public class ProjectListQuery
    {
        public string? Search { get; set; }

        public DateTime? DueDate { get; set; }

        public string? IsPublished { get; set; }

        public string? IsDeleted { get; set; }

        public int? CompanyId{ get; set; }

        public int Page { get; set; } = 1;

        public int PageSize { get; set; } = 10;

        public string? SortBy { get; set; } = "CreatedAt";

        public string? SortDirection { get; set; } = "desc";
    }

    public class ProjectIsDeletedRequest
    {
        public string IsDeleted { get; set; } = "N";
    }

    public class UpdateProjectRequest
    {
        [Required]
        [MaxLength(150)]
        public string ProjectName { get; set; } = null!;

        public string? Description { get; set; }

        public DateTime? DueDate { get; set; }
    }

    public class CreateProjectRequest
    {
        [Required]
        public string ProjectName { get; set; } = null!;

        public string? Description { get; set; }
    }
}
