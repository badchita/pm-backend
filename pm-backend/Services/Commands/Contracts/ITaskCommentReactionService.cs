using pm_backend.DTOs;

namespace pm_backend.Services.Commands.Contracts
{
    public interface ITaskCommentReactionService
    {
        Task UpdateTaskCommentReactionAsync(TaskCommentReactionDTO taskCommentReaction, int taskId, int taskCommentId);
    }
}
