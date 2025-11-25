using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using pm_backend.Data;
using pm_backend.DTOs;
using pm_backend.Models;
using pm_backend.Services;
using System;

namespace pm_backend.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly PmDbContext _context;

        public AuthController(PmDbContext context)
        {
            _context = context;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            var exists = await _context.Users.AnyAsync(u => u.Email == request.Email);
            if (exists)
                return BadRequest("Email already registered");

            var user = new User
            {
                Name = request.Name,
                Email = request.Email,
                PasswordHash = PasswordService.Hash(request.Password)
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok("User registered successfully");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == request.Email);

            if (user == null)
                return Unauthorized("Invalid email or password");

            var validPassword = PasswordService.Verify(request.Password, user.PasswordHash);

            if (!validPassword)
                return Unauthorized("Invalid email or password");

            return Ok(new
            {
                message = "Login success",
                user.Id,
                user.Name,
                user.Email
            });
        }
    }
}
