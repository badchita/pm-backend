namespace pm_backend.DTOs
{
    public class CreateCompanyRequest
    {
        public int? Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? CompanyEmail { get; set; }
        public string? IsApproved { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}
