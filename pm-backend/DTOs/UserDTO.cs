using pm_backend.Models;

namespace pm_backend.DTOs
{
    public class UserDto
    {
        public int Id { get; set; }

        public string Name { get; set; } = "";

        public string Email { get; set; } = "";

        public string IsApproved { get; set; } = "";

        public UserRole Role { get; set; }

        public int? CompanyId { get; set; }

        public Company? Company { get; set; }
    }

    public class UserIsDeletedRequest
    {
        public string IsDeleted { get; set; } = "N";
    }

    public class UserListQuery
    {
        public string? Search { get; set; }

        public string? IsApproved { get; set; }

        public int? CompanyId { get; set; }

        public string? Role { get; set; }

        public string? IsDeleted { get; set; }
        public int? ExcludeUserId { get; set; }

        public int Page { get; set; } = 1;

        public int PageSize { get; set; } = 10;

        public string? SortBy { get; set; } = "CreatedAt";

        public string? SortDirection { get; set; } = "desc";
    }
}
