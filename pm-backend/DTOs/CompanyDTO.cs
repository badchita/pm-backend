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

    public class CompanyListQuery
    {
        public string? Search { get; set; }

        public string? IsApproved { get; set; }

        public string? IsDeleted { get; set; }

        public int Page { get; set; } = 1;

        public int PageSize { get; set; } = 10;

        public string? SortBy { get; set; } = "CreatedAt";

        public string? SortDirection { get; set; } = "desc";
    }
}
