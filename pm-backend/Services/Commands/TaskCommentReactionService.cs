using Microsoft.EntityFrameworkCore;
using pm_backend.Data;
using pm_backend.DTOs;
using pm_backend.Models;
using pm_backend.Services.Commands.Contracts;

namespace pm_backend.Services.Commands
{
    public class TaskCommentReactionService : ITaskCommentReactionService
    {
        private readonly PmDbContext _context;

        public TaskCommentReactionService(PmDbContext context)
        {
            _context = context;
        }

        public async Task UpdateTaskCommentReactionAsync(TaskCommentReactionRequest taskCommentReaction)
        {
            var commentExists = await _context.TaskComments
                .AnyAsync(c => c.Id == taskCommentReaction.TaskCommentId);

            if (!commentExists)
                throw new KeyNotFoundException("Task comment does not exist.");

            var existingReaction = await _context.TaskCommentReactions
                .FirstOrDefaultAsync(r =>
                    r.Id == taskCommentReaction.Id);

            if (taskCommentReaction.ReactionType == null)
            {
                if (existingReaction != null)
                {
                    _context.TaskCommentReactions.Remove(existingReaction);
                    await _context.SaveChangesAsync();
                }

                return;
            }

            if (existingReaction == null)
            {
                var reaction = new TaskCommentReaction
                {
                    TaskCommentId = taskCommentReaction.TaskCommentId,
                    UserId = taskCommentReaction.UserId,
                    ReactionType = taskCommentReaction.ReactionType,
                    CreatedAt = DateTime.UtcNow
                };

                _context.TaskCommentReactions.Add(reaction);
            }
            else
            {
                existingReaction.ReactionType = taskCommentReaction.ReactionType;
            }

            await _context.SaveChangesAsync();
        }
    }
}
