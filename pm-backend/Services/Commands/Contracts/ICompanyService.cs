using pm_backend.DTOs;
using pm_backend.Models;

namespace pm_backend.Services.Commands.Contracts
{
    public interface ICompanyService
    {
        Task<Company> CreateCompanyAsync(CreateCompanyRequest request);
        Task UpdateIsDeletedAsync(int id, string isDeleted);
        Task<Company?> GetCompanyByIdAsync(int id);
        Task<Company?> UpdateCompanyAsync(int id, CreateCompanyRequest request);
    }
}
