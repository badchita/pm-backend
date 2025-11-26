using Azure.Core;
using Microsoft.EntityFrameworkCore;
using pm_backend.Data;
using pm_backend.DTOs;
using pm_backend.Models;
using pm_backend.Services.Commands.Contracts;

namespace pm_backend.Services.Commands
{
    public class AuthService : IAuthService
    {
        private readonly PmDbContext _context;

        public AuthService(PmDbContext context) 
        {
            _context = context;
        }

        public async Task<User> RegisterUser(RegisterRequest registerForm)
        {
            try
            {
                var exists = await _context.Users
                    .AnyAsync(x => x.Email == registerForm.Email);

                if (exists)
                    throw new InvalidOperationException("Email already registered");

                var user = new User
                {
                    Name = registerForm.Name,
                    Email = registerForm.Email,
                    PasswordHash = PasswordService.Hash(registerForm.Password)
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                return user;
            }
            catch (InvalidOperationException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception("Internal server error occurred while registering the user.", ex);
            }
        }
    }
}
