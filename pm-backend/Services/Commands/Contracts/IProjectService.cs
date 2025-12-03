using pm_backend.DTOs;
using pm_backend.Models;

namespace pm_backend.Services.Commands.Contracts
{
    public interface IProjectService
    {
        Task<Project> CreateProject(CreateProjectRequest request, string userEmail);
        Task<Project?> GetProjectByIdAsync(int id, string userEmail);
    }
}
