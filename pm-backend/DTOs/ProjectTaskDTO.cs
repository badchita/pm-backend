using pm_backend.Models;

namespace pm_backend.DTOs
{
    public class CreateProjectTaskRequest
    {
        public int? Id { get; set; }

        public int ProjectId { get; set; }

        public string TaskName { get; set; } = string.Empty;

        public string? Description { get; set; }

        public string? AcceptanceCriteria { get; set; }

        public string? AssignedTo { get; set; }

        public int? TaskPoints { get; set; }

        public int State { get; set; }

        public DateTime? ReadyForDevelopmentDate { get; set; }

        public DateTime? DoneDate { get; set; }

        public DateTime? TestingStartDate { get; set; }

        public DateTime? TestingEndDate { get; set; }
    }

    public class ProjectTaskListQuery
    {
        public int Page { get; set; } = 1;

        public int PageSize { get; set; } = 10;

        public string? Search { get; set; }

        public string? State { get; set; }

        public string? AssignedTo { get; set; }

        public string? SortBy { get; set; } = "createdAt";

        public string? SortDirection { get; set; } = "desc";
    }

    public class UpdateTaskStateRequest
    {
        public TaskState State { get; set; }
    }
}
