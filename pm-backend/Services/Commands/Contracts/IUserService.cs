namespace pm_backend.Services.Commands.Contracts
{
    public interface IUserService
    {
        Task UpdateIsDeletedAsync(int id, string isDeleted);
    }
}
