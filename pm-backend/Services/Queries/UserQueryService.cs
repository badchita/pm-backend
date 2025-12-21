using Microsoft.EntityFrameworkCore;
using pm_backend.Data;
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

        public async Task<IReadOnlyList<User>> GetAllUsers(string? search)
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
    }
}
