using pm_backend.Models;

namespace pm_backend.Services.Queries.Contracts
{
    public interface ITaskCommentQueryService
    {
        Task<IReadOnlyList<TaskComment>> GetAllTaskCommentsAsync(int taskId);
    }
}
