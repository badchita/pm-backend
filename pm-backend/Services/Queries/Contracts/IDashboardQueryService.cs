using pm_backend.DTOs;

namespace pm_backend.Services.Queries.Contracts
{
    public interface IDashboardQueryService
    {
        Task<Dashboard> GetDashboardAsync(string userEmail);
    }
}
