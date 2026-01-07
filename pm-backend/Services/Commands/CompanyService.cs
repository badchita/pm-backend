using pm_backend.Data;
using pm_backend.DTOs;
using pm_backend.Models;
using pm_backend.Services.Commands.Contracts;

namespace pm_backend.Services.Commands
{
    public class CompanyService : ICompanyService
    {
        private readonly PmDbContext _context;

        public CompanyService(
            PmDbContext context
        ) 
        {
            _context = context;
        }

        public async Task<Company> CreateCompanyAsync(CreateCompanyRequest request)
        {
            var company = new Company
            {
                Name = request.Name,
                CreatedAt = DateTime.UtcNow,
            };

            _context.Companies.Add(company);
            await _context.SaveChangesAsync();

            return company;
        }
    }
}
