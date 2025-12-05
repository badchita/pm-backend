namespace pm_backend.DTOs
{
    public class PublishProjectRequest
    {
        public string? ProjectName { get; set; }
        public string? Description { get; set; }
        public DateTime? DueDate { get; set; }
    }
}
