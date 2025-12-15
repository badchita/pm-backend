using pm_backend.DTOs.Tasks;
using pm_backend.Models;

namespace pm_backend.Services.Commands.Contracts
{
    public interface IProjectTaskService
    {
        Task<ProjectTask> CreateTaskAsync(CreateProjectTaskRequest request, string userEmail);
        Task<ProjectTask?> GetTaskByIdAsync(int id, int projectId, string userEmail)
    }
}
