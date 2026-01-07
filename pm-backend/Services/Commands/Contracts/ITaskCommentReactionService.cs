using pm_backend.DTOs;

namespace pm_backend.Services.Commands.Contracts
{
    public interface ITaskCommentReactionService
    {
        Task UpdateTaskCommentReactionAsync(TaskCommentReactionRequest taskCommentReaction, int taskId, int taskCommentId);
    }
}
