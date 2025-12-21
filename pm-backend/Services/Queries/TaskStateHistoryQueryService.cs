using Microsoft.EntityFrameworkCore;
using pm_backend.Data;
using pm_backend.Models;

namespace pm_backend.Services.Queries
{
    public class TaskStateHistoryQueryService
    {
        private readonly PmDbContext _context;

        public TaskStateHistoryQueryService(PmDbContext context)
        {
            _context = context;
        }

        public async Task<IReadOnlyList<TaskStateHistory>> GetAllTaskStateHistoryAsync(int taskId)
        {
            return await _context.TaskStateHistories
                .Where(h => h.TaskId == taskId)
                .OrderByDescending(h => h.ChangedAt)
                .ToListAsync();
        }
    }
}
