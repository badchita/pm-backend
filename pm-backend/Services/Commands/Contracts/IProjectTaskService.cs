using pm_backend.DTOs;
using pm_backend.Models;

namespace pm_backend.Services.Commands.Contracts
{
    public interface IProjectTaskService
    {
        Task<ProjectTask> CreateTaskAsync(CreateProjectTaskRequest request, string userEmail);
        Task<ProjectTask?> GetTaskByIdAsync(int id, int projectId, string userEmail);
        Task<ProjectTask?> UpdateTaskAsync(int id, int projectId, CreateProjectTaskRequest request, string userEmail);
        Task UpdateTaskStateAsync(int taskId, TaskState newState, string userEmail);
        Task DeleteTaskAsync(int taskId);
    }
}
