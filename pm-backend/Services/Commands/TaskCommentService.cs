using Microsoft.EntityFrameworkCore;
using pm_backend.Data;
using pm_backend.DTOs;
using pm_backend.Models;
using pm_backend.Services.Commands.Contracts;

namespace pm_backend.Services.Commands
{
    public class TaskCommentService : ITaskCommentService
    {
        private readonly PmDbContext _context;

        public TaskCommentService(PmDbContext context)
        {
            _context = context;
        }

        public async Task<TaskComment> CreateTaskCommentAsync(CreateTaskCommentRequest request)
        {
            var task = await _context.ProjectTasks.FirstOrDefaultAsync(t => t.Id == request.TaskId);

            if (task == null)
                throw new KeyNotFoundException("Task not found.");

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == request.UserId);

            if (user == null)
                throw new KeyNotFoundException("User not found.");

            var taskComment = new TaskComment
            {
                Content = request.Content,
                UserId = request.UserId,
                TaskId = request.TaskId,
                CreatedAt = DateTime.UtcNow
            };

            _context.TaskComments.Add(taskComment);
            await _context.SaveChangesAsync();

            await _context.Entry(taskComment).Reference(c => c.User).LoadAsync();
            await _context.Entry(taskComment).Reference(c => c.Task).LoadAsync();

            return taskComment;
        }
    }
}
