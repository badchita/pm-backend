using pm_backend.DTOs;
using pm_backend.Models;

namespace pm_backend.Services.Queries.Contracts
{
    public interface IProjectTaskQueryService
    {
        Task<PagedResult<ProjectTask>> GetProjectTasksAsync(int projectId,ProjectTaskListQuery query,string userEmail);
    }
}
