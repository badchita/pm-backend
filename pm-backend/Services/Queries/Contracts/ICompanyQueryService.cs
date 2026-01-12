using pm_backend.DTOs;
using pm_backend.Models;

namespace pm_backend.Services.Queries.Contracts
{
    public interface ICompanyQueryService
    {
        Task<IReadOnlyList<Company>> GetAllCompaniesAsync(string? search);
        Task<PagedResult<Company>> GetCompaniesAsync(CompanyListQuery query);
        Task<PagedResult<User>> GetCompanyUsersAsync(int companyId, UserListQuery query);
        Task<PagedResult<User>> GetUserMembersAsync(UserListQuery query, string userEmail, int companyId);
    }
}
