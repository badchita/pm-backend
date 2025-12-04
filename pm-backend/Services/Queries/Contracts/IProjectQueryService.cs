using pm_backend.DTOs;
using pm_backend.Models;

namespace pm_backend.Services.Queries.Contracts
{
    public interface IProjectQueryService
    {
        Task<PagedResult<Project>> GetProjectsAsync(ProjectListQuery query, string userEmail);
    }
}
