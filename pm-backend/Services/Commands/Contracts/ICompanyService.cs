using pm_backend.DTOs;
using pm_backend.Models;

namespace pm_backend.Services.Commands.Contracts
{
    public interface ICompanyService
    {
        Task<Company> CreateCompanyAsync(CreateCompanyDTO request);
    }
}
