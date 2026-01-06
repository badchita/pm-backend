using Microsoft.EntityFrameworkCore;
using pm_backend.Data;
using pm_backend.Models;
using pm_backend.Services.Queries.Contracts;

namespace pm_backend.Services.Queries
{
    public class CompanyQueryService : ICompanyQueryService
    {
        private readonly PmDbContext _context;

        public CompanyQueryService(PmDbContext context)
        {
            _context = context;
        }

        public async Task<IReadOnlyList<Company>> GetAllCompaniesAsync(string? search)
        {
            IQueryable<Company> query = _context.Companies;

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(u =>
                    u.Name.Contains(search)
                );
            }

            return await query.ToListAsync();
        }
    }
}
