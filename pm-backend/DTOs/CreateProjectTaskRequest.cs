namespace pm_backend.DTOs.Tasks
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
}
