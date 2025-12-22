using Microsoft.EntityFrameworkCore;
using pm_backend.Data;
using pm_backend.Services.Queries.Contracts;

namespace pm_backend.Services.Queries
{
    public class TaskStateHistoryQueryService : ITaskStateHistoryQueryService
    {
        private readonly PmDbContext _context;

        public TaskStateHistoryQueryService(PmDbContext context)
        {
            _context = context;
        }

        public async Task<IReadOnlyList<TaskStateHistory>> GetAllTaskStateHistoryAsync(int taskId)
        {
            var taskHistories = await _context.TaskStateHistories
                .Where(h => h.TaskId == taskId)
                .OrderByDescending(h => h.ChangedAt)
                .ToListAsync();

            var emails = taskHistories
                .Select(h => h.ChangedBy)
                .Distinct()
                .ToList();

            var users = await _context.Users
                .Where(u => emails.Contains(u.Email))
                .ToDictionaryAsync(u => u.Email);

            foreach (var history in taskHistories)
            {
                if (users.TryGetValue(history.ChangedBy, out var user))
                {
                    history.ChangedBy = user.Name;
                }
            }

            return taskHistories;
        }
    }
}
