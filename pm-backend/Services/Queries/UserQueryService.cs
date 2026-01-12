using Microsoft.EntityFrameworkCore;
using pm_backend.Data;
using pm_backend.DTOs;
using pm_backend.Models;
using pm_backend.Services.Queries.Contracts;

namespace pm_backend.Services.Queries
{
    public class UserQueryService : IUserQueryService
    {
        private readonly PmDbContext _context;

        public UserQueryService(PmDbContext context)
        {
            _context = context;
        }

        public async Task<IReadOnlyList<User>> GetAllUsersAsync(string? search)
        {
            IQueryable<User> query = _context.Users;

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(u =>
                    u.Name.Contains(search) ||
                    u.Email.Contains(search)
                );
            }

            return await query.ToListAsync();
        }

        public async Task<PagedResult<User>> GetUsersAsync(UserListQuery query, string userEmail)
        {
            IQueryable<User> users = _context.Users
                .Include(u => u.Company)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(userEmail))
            {
                users = users.Where(u => u.Email != userEmail);
            }

            if (!string.IsNullOrWhiteSpace(query.Search))
            {
                users = users.Where(u =>
                    u.Name.Contains(query.Search) ||
                    u.Email.Contains(query.Search)
                );
            }

            if (!string.IsNullOrWhiteSpace(query.IsApproved))
            {
                users = users.Where(u => u.IsApproved == query.IsApproved);
            }

            if (query.CompanyId.HasValue)
            {
                users = users.Where(u => u.CompanyId == query.CompanyId);
            }

            if (!string.IsNullOrWhiteSpace(query.Role))
            {
                if (Enum.TryParse<UserRole>(query.Role, ignoreCase: true, out var roleFilter))
                {
                    users = users.Where(u => u.Role == roleFilter);
                }
            }

            if (!string.IsNullOrWhiteSpace(query.IsDeleted))
            {
                users = users.Where(u => u.IsDeleted == query.IsDeleted);
            }

            var sortBy = query.SortBy?.Trim().ToLower();
            var sortDirection = query.SortDirection?.Trim().ToLower() == "asc" ? "asc" : "desc";

            users = sortBy switch
            {
                "name" => sortDirection == "asc"
                    ? users.OrderBy(u => u.Name)
                    : users.OrderByDescending(u => u.Name),

                "email" => sortDirection == "asc"
                    ? users.OrderBy(u => u.Email)
                    : users.OrderByDescending(u => u.Email),

                "createdat" => sortDirection == "asc"
                    ? users.OrderBy(u => u.CreatedAt)
                    : users.OrderByDescending(u => u.CreatedAt),

                _ => users.OrderByDescending(u => u.CreatedAt)
            };

            var totalCount = await users.CountAsync();

            var data = await users
                .Skip((query.Page - 1) * query.PageSize)
                .Take(query.PageSize)
                .ToListAsync();

            return new PagedResult<User>
            {
                Data = data,
                TotalCount = totalCount,
                Page = query.Page,
                PageSize = query.PageSize
            };
        }

        public async Task<PagedResult<User>> GetUserMembersAsync(UserListQuery query, string userEmail, int companyId)
        {
            var companyExists = await _context.Companies
                .AnyAsync(c => c.Id == companyId);

            if (!companyExists)
                throw new KeyNotFoundException($"Project with id {companyId} not found.");

            IQueryable<User> users = _context.Users
            .Include(u => u.Company)
            .AsQueryable();

            if (!string.IsNullOrWhiteSpace(userEmail))
            {
                users = users.Where(u => u.Email != userEmail);
            }

            if (companyId > 0)
            {
                users = users.Where(u => u.CompanyId == companyId);
            }

            if (!string.IsNullOrWhiteSpace(query.Search))
            {
                users = users.Where(u =>
                    u.Name.Contains(query.Search) ||
                    u.Email.Contains(query.Search)
                );
            }

            if (!string.IsNullOrWhiteSpace(query.IsApproved))
            {
                users = users.Where(u => u.IsApproved == query.IsApproved);
            }

            if (!string.IsNullOrWhiteSpace(query.Role))
            {
                if (Enum.TryParse<UserRole>(query.Role, ignoreCase: true, out var roleFilter))
                {
                    users = users.Where(u => u.Role == roleFilter);
                }
            }

            if (!string.IsNullOrWhiteSpace(query.IsDeleted))
            {
                users = users.Where(u => u.IsDeleted == query.IsDeleted);
            }

            var sortBy = query.SortBy?.Trim().ToLower();
            var sortDirection = query.SortDirection?.Trim().ToLower() == "asc" ? "asc" : "desc";

            users = sortBy switch
            {
                "name" => sortDirection == "asc"
                    ? users.OrderBy(u => u.Name)
                    : users.OrderByDescending(u => u.Name),

                "email" => sortDirection == "asc"
                    ? users.OrderBy(u => u.Email)
                    : users.OrderByDescending(u => u.Email),

                "createdat" => sortDirection == "asc"
                    ? users.OrderBy(u => u.CreatedAt)
                    : users.OrderByDescending(u => u.CreatedAt),

                _ => users.OrderByDescending(u => u.CreatedAt)
            };

            var totalCount = await users.CountAsync();

            var data = await users
                .Skip((query.Page - 1) * query.PageSize)
                .Take(query.PageSize)
                .ToListAsync();

            return new PagedResult<User>
            {
                Data = data,
                TotalCount = totalCount,
                Page = query.Page,
                PageSize = query.PageSize
            };
        }
    }
}
