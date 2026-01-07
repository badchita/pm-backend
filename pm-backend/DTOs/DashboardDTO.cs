namespace pm_backend.DTOs
{
    public class Dashboard
    {
        public int ActiveProjectsCount { get; set; }

        public List<ActiveProjectProgressDto> ActiveProjects { get; set; } = [];

        public List<UpcomingProjectDto> UpcomingDeadlines { get; set; } = [];

        public List<TasksCompletedByProjectDto> TasksCompletedByProject { get; set; } = [];
    }

    public class ActiveProjectProgressDto
    {
        public int ProjectId { get; set; }

        public string ProjectName { get; set; } = string.Empty;

        public DateTime? DueDate { get; set; }

        public int TotalTasks { get; set; }

        public int ClosedTasks { get; set; }

        public int ProgressPercentage { get; set; }
    }

    public class UpcomingProjectDto
    {
        public int ProjectId { get; set; }

        public string ProjectName { get; set; } = string.Empty;

        public DateTime? DueDate { get; set; }

        public int RemainingDays { get; set; }
    }

    public class TasksCompletedByProjectDto
    {
        public string ProjectName { get; set; } = string.Empty;

        public int CompletedTasks { get; set; }
    }
}
