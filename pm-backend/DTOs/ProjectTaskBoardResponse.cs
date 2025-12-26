using pm_backend.Models;

namespace pm_backend.DTOs
{
    public class ProjectTaskBoardResponse
    {
        public int Id { get; set; }
        public string ProjectIdNumber { get; set; } = null!;
        public string ProjectName { get; set; } = null!;
        public IEnumerable<ProjectTaskDto> Tasks { get; set; } = new List<ProjectTaskDto>();
    }

    public class ProjectTaskDto
    {
        public int Id { get; set; }
        public string TaskName { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string AssignedTo { get; set; } = null!;
        public string TaskIdNumber { get; set; } = null!;
        public TaskState State { get; set; }
        public string StateLabel => State.ToString();
        public string CreatedBy { get; set; } = null!;
        public string UpdatedBy { get; set; } = null!;
        public int ProjectId { get; set; }
    }
}
