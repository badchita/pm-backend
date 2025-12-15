namespace pm_backend.DTOs.Tasks
{
    public class ProjectTaskListQuery
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;

        public string? Search { get; set; }
        public string? State { get; set; }

        public string? SortBy { get; set; } = "createdAt";
        public string? SortDirection { get; set; } = "desc";
    }
}
