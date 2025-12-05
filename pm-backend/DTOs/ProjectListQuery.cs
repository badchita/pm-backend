namespace pm_backend.DTOs
{
    public class ProjectListQuery
    {
        public string? Search { get; set; }
        public DateTime? DueDate { get; set; }
        public string? IsPublished { get; set; }

        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;

        public string? SortBy { get; set; } = "CreatedAt";
        public string? SortDirection { get; set; } = "desc";
    }
}
