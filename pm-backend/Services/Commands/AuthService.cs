using Azure.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using pm_backend.Data;
using pm_backend.DTOs;
using pm_backend.Models;
using pm_backend.Services.Commands.Contracts;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
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

                if (registerForm.Role == UserRole.Manager && !string.IsNullOrWhiteSpace(registerForm.CompanyName))
                {
                    var company = new Company
                    {
                        Name = registerForm.CompanyName,
                        IsApproved = "N",
                        CreatedAt = DateTime.UtcNow
                    };

                    _context.Companies.Add(company);
                    await _context.SaveChangesAsync();

                    user.CompanyId = company.Id;
                }

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
                .Include(u => u.Company)
                .FirstOrDefaultAsync(u => u.Email == request.Email);

            if (user == null)
                throw new UnauthorizedAccessException("Invalid email or password");

            var validPassword = PasswordService.Verify(request.Password, user.PasswordHash);

            if (!validPassword)
                throw new UnauthorizedAccessException("Invalid email or password");

            var isApprove = user.IsApproved;

            if (isApprove == "N")
                throw new UnauthorizedAccessException("User not yet approved.");

            var accessToken = GenerateJwtToken(user);
            var refreshToken = GenerateRefreshToken();

            _context.RefreshTokens.Add(new RefreshToken
            {
                UserId = user.Id,
                Token = refreshToken,
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddDays(7)
            });

            await _context.SaveChangesAsync();

            return new LoginResponse
            {
                Token = accessToken,
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

        public async Task<LoginResponse> RefreshToken(string refreshToken)
        {
            var token = await _context.RefreshTokens
                .Include(rt => rt.User)
                .FirstOrDefaultAsync(rt => rt.Token == refreshToken);

            if (token == null || !token.IsActive)
                throw new UnauthorizedAccessException("Invalid refresh token");

            var newRefreshToken = GenerateRefreshToken();

            token.RevokedAt = DateTime.UtcNow;
            token.ReplacedByToken = newRefreshToken;

            _context.RefreshTokens.Add(new RefreshToken
            {
                UserId = token.UserId,
                Token = newRefreshToken,
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddDays(7)
            });

            await _context.SaveChangesAsync();

            return new LoginResponse
            {
                Token = GenerateJwtToken(token.User),
                RefreshToken = newRefreshToken
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
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(30),
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

        private static string GenerateRefreshToken()
        {
            var randomBytes = RandomNumberGenerator.GetBytes(64);
            return Convert.ToBase64String(randomBytes);
        }

    }
}
