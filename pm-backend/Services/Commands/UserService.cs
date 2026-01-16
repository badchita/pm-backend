using Microsoft.EntityFrameworkCore;
using pm_backend.Data;
using pm_backend.DTOs;
using pm_backend.Models;
using pm_backend.Services.Commands.Contracts;

namespace pm_backend.Services.Commands
{
    public class UserService : IUserService
    {
        private readonly PmDbContext _context;

        public UserService(PmDbContext context)
        {
            _context = context;
        }

        public async Task UpdateIsDeletedAsync(int id, string isDeleted)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
                throw new KeyNotFoundException("User not found.");

            user.IsDeleted = isDeleted;

            if (isDeleted == "Y")
            {
                user.IsApproved = "N";
            }

            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task<User?> GetUserByIdAsync(int id)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == id && u.IsDeleted == "N");

            return user;
        }

        public async Task<User?> UpdateUserAsync(int id, UserDto request)
        {

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id && u.IsDeleted == "N");

            if (user == null)
                throw new KeyNotFoundException("User not found.");

            user.Name = request.Name;
            user.Email = request.Email;
            user.IsApproved = request.IsApproved;
            user.Role = request.Role;
            user.CompanyId = request.CompanyId;

            user.Email = request.Email;

            if (request.CompanyId > 0)
            {
                var company = await _context.Companies
                    .FirstOrDefaultAsync(c => c.Id == request.CompanyId && c.IsDeleted == "N");

                if (company == null)
                    throw new KeyNotFoundException("Company not found.");

                var currentEmail = request.Email;
                var userNamePart = currentEmail.Split('@')[0];
                var companyEmail = company.CompanyEmail;

                string companyDomain;

                if (companyEmail.Contains("@"))
                {
                    companyDomain = "@" + companyEmail.Split('@')[1];
                }
                else
                {
                    companyDomain = companyEmail.StartsWith("@")
                        ? companyEmail
                        : "@" + companyEmail;
                }

                user.Email = $"{userNamePart}{companyDomain}";
            }

            await _context.SaveChangesAsync();

            return user;
        }
    }
}
