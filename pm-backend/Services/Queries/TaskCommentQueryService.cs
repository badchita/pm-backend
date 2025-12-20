using Microsoft.EntityFrameworkCore;
using pm_backend.Data;
using pm_backend.Models;
using pm_backend.Services.Queries.Contracts;

namespace pm_backend.Services.Queries
{
    public class TaskCommentQueryService : ITaskCommentQueryService
    {
        private readonly PmDbContext _context;

        public TaskCommentQueryService(PmDbContext context)
        {
            _context = context;
        }

        public async Task<IReadOnlyList<TaskComment>> GetAllTaskCommentsAsync(int taskId)
        {
            return await _context.TaskComments
              .Where(c => c.TaskId == taskId)
              .Include(c => c.User)
              .Include(c => c.Task)
              .OrderByDescending(c => c.CreatedAt)
              .AsNoTracking()
              .ToListAsync();
        }
    }
}
