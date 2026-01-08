using Microsoft.EntityFrameworkCore;
using pm_backend.Data;
using pm_backend.DTOs;
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

        public async Task<PagedResult<Company>> GetCompaniesAsync(CompanyListQuery query)
        {
            IQueryable<Company> companies = _context.Companies.AsQueryable();

            if (!string.IsNullOrWhiteSpace(query.Search))
            {
                companies = companies.Where(c =>
                    c.Name.Contains(query.Search) ||
                    c.CompanyEmail.Contains(query.Search)
                );
            }

            if (!string.IsNullOrWhiteSpace(query.IsApproved))
            {
                companies = companies.Where(c => c.IsApproved == query.IsApproved);
            }

            if (!string.IsNullOrWhiteSpace(query.IsDeleted))
            {
                companies = companies.Where(c => c.IsDeleted == query.IsDeleted);
            }

            var sortBy = query.SortBy?.Trim().ToLower();
            var sortDirection = query.SortDirection?.Trim().ToLower() == "asc" ? "asc" : "desc";

            companies = sortBy switch
            {
                "name" => sortDirection == "asc"
                    ? companies.OrderBy(c => c.Name)
                    : companies.OrderByDescending(c => c.Name),

                "email" => sortDirection == "asc"
                    ? companies.OrderBy(c => c.CompanyEmail)
                    : companies.OrderByDescending(c => c.CompanyEmail),

                "createdat" => sortDirection == "asc"
                    ? companies.OrderBy(c => c.CreatedAt)
                    : companies.OrderByDescending(c => c.CreatedAt),

                _ => companies.OrderByDescending(c => c.CreatedAt)
            };

            var totalCount = await companies.CountAsync();

            var data = await companies
                .Skip((query.Page - 1) * query.PageSize)
                .Take(query.PageSize)
                .ToListAsync();

            return new PagedResult<Company>
            {
                Data = data,
                TotalCount = totalCount,
                Page = query.Page,
                PageSize = query.PageSize
            };
        }
    }
}
