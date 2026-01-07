using pm_backend.DTOs;
using pm_backend.Models;

namespace pm_backend.Services.Commands.Contracts
{
    public interface ITaskCommentService
    {
        Task<TaskComment> CreateTaskCommentAsync(TaskCommentDTO request, int taskId);
    }
}
