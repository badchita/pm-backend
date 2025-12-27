using pm_backend.Data;
using pm_backend.Models;
using pm_backend.Services.Commands.Contracts;

namespace pm_backend.Services.Commands
{
    public class TaskStateHistoryService : ITaskStateHistoryService
    {
        private readonly PmDbContext _context;

        public TaskStateHistoryService(PmDbContext context)
        {
            _context = context;
        }

        public async Task TrackStateChangeAsync(int taskId,TaskState previousState,TaskState newState, string changedBy)
        {
            if (previousState == newState)
                return;

            var history = new TaskStateHistory
            {
                TaskId = taskId,
                PreviousState = previousState,
                NewState = newState,
                ChangedBy = changedBy,
                ChangedAt = DateTime.UtcNow
            };

            _context.TaskStateHistories.Add(history);

            await Task.CompletedTask;
        }
    }
}
