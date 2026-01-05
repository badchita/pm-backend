using Azure.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using pm_backend.Data;
using pm_backend.DTOs;
using pm_backend.Models;
using pm_backend.Services.Commands.Contracts;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace pm_backend.Services.Commands
{
    public class AuthService : IAuthService
    {
        private readonly PmDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthService(
            PmDbContext context,
            IConfiguration configuration
        ) 
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<User> RegisterUser(RegisterRequest registerForm)
        {
            try
            {
                var exists = await _context.Users
                    .AnyAsync(u => u.Email == registerForm.Email);

                if (exists)
                    throw new InvalidOperationException("Email already registered");

                var user = new User
                {
                    Name = registerForm.Name,
                    Email = registerForm.Email,
                    Role = registerForm.Role,
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

        public async Task<LoginResponse> Login(LoginRequest request)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == request.Email);

            if (user == null)
                throw new UnauthorizedAccessException("Invalid email or password");

            var validPassword = PasswordService.Verify(request.Password, user.PasswordHash);

            if (!validPassword)
                throw new UnauthorizedAccessException("Invalid email or password");

            var isApprove = user.IsApproved;

            if (isApprove == "N")
                throw new UnauthorizedAccessException("User not yet approved.");

            var token = GenerateJwtToken(user);

            return new LoginResponse
            {
                Token = token,
                User = new UserDto
                {
                    Id = user.Id,
                    Name = user.Name,
                    Email = user.Email,
                    Role = user.Role,
                    IsApproved = user.IsApproved,
                    CompanyId = user.CompanyId,
                    Company = user.Company,
                }
            };
        }

        private string GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Email, user.Email),
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(6),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"],
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature
                )
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
