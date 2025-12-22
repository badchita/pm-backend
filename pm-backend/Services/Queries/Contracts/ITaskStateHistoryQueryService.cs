namespace pm_backend.Services.Queries.Contracts
{
    public interface ITaskStateHistoryQueryService
    {
        Task<IReadOnlyList<TaskStateHistory>> GetAllTaskStateHistoryAsync(int taskId);
    }
}
