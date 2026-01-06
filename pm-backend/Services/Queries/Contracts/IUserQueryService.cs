using pm_backend.DTOs;
using pm_backend.Models;

namespace pm_backend.Services.Queries.Contracts
{
    public interface IUserQueryService
    {
        Task<IReadOnlyList<User>> GetAllUsersAsync(string? search);
        Task<PagedResult<User>> GetUsersAsync(UserListQuery query, string userEmail);
    }
}
