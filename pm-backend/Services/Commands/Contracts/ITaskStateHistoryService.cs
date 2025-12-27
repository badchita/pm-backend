using pm_backend.Models;

namespace pm_backend.Services.Commands.Contracts
{
    public interface ITaskStateHistoryService
    {
        Task TrackStateChangeAsync(int taskId, TaskState previousState, TaskState newState, string changedBy);
    }
}
