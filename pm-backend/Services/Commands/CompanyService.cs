using Microsoft.EntityFrameworkCore;
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

        public async Task UpdateIsDeletedAsync(int id, string isDeleted)
        {
            var company = await _context.Companies.FindAsync(id);

            if (company == null)
                throw new KeyNotFoundException("Company not found.");

            company.IsDeleted = isDeleted;

            if (isDeleted == "Y")
            {
                company.IsApproved = "N";
            }

            _context.Companies.Update(company);
            await _context.SaveChangesAsync();
        }

        public async Task<Company?> GetCompanyByIdAsync(int id)
        {
            var company = await _context.Companies
                .FirstOrDefaultAsync(u => u.Id == id && u.IsDeleted == "N");

            if (company == null)
                throw new KeyNotFoundException("Company not found.");

            return company;
        }

        public async Task<Company?> UpdateCompanyAsync(int id, CreateCompanyRequest request)
        {

            var company = await _context.Companies.FirstOrDefaultAsync(c => c.Id == id && c.IsDeleted == "N");

            if (company == null)
                throw new KeyNotFoundException("Company not found.");

            company.Name = request.Name;
            company.CompanyEmail = request.CompanyEmail;
            company.IsApproved = request.IsApproved;

            await _context.SaveChangesAsync();

            return company;
        }
    }
}
