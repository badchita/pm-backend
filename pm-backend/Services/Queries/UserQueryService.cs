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
            IQueryable<User> users = _context.Users.AsQueryable();

            if (!string.IsNullOrWhiteSpace(query.Search))
            {
                users = users.Where(p =>
                    p.Name.Contains(query.Search) ||
                    p.Email.Contains(query.Search)
                );
            }

            if (!string.IsNullOrWhiteSpace(query.IsApproved))
            {
                users = users.Where(p => p.IsApproved == query.IsApproved);
            }

            if (query.CompanyId.HasValue)
            {
                users = users.Where(p => p.CompanyId == query.CompanyId);
            }

            if (!string.IsNullOrWhiteSpace(query.Role))
            {
                if (Enum.TryParse<UserRole>(query.Role, ignoreCase: true, out var roleFilter))
                {
                    users = users.Where(p => p.Role == roleFilter);
                }
            }

            var sortBy = query.SortBy?.Trim().ToLower();
            var sortDirection = query.SortDirection?.Trim().ToLower() == "asc" ? "asc" : "desc";

            users = sortBy switch
            {
                "name" => sortDirection == "asc"
                    ? users.OrderBy(p => p.Name)
                    : users.OrderByDescending(p => p.Name),

                "email" => sortDirection == "asc"
                    ? users.OrderBy(p => p.Email)
                    : users.OrderByDescending(p => p.Email),

                "createdat" => sortDirection == "asc"
                    ? users.OrderBy(p => p.CreatedAt)
                    : users.OrderByDescending(p => p.CreatedAt),

                _ => users.OrderByDescending(p => p.CreatedAt)
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
