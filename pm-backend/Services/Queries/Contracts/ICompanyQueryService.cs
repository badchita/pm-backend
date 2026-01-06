using pm_backend.Models;

namespace pm_backend.Services.Queries.Contracts
{
    public interface ICompanyQueryService
    {
        Task<IReadOnlyList<Company>> GetAllCompaniesAsync(string? search);
    }
}
