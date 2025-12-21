using pm_backend.Models;

namespace pm_backend.Services.Queries.Contracts
{
    public interface IUserQueryService
    {
        Task<IReadOnlyList<User>> GetAllUsers(string search);
    }
}
