using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using pm_backend.Data;
using pm_backend.Services.Commands;
using pm_backend.Services.Commands.Contracts;
using System.Text;

namespace pm_backend
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services
            builder.Services.AddRazorPages();

            // Add controller support for Web API
            builder.Services.AddControllers();

            // Add DbContext
            builder.Services.AddDbContext<PmDbContext>(options =>
            {
                options.UseSqlServer(
                   "Server=localhost;Database=pm;Trusted_Connection=True;TrustServerCertificate=True;"
                );
            });

            // Configure JWT Authentication
            var jwtKey = builder.Configuration["Jwt:Key"]
                         ?? "TfW2gR+gL0mmYnNtePOX+0/sTbQcRB9A7JiGo8QmR/WA=";

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration["JwtSettings:Issuer"] ?? "pm-backend",
                    ValidAudience = builder.Configuration["JwtSettings:Audience"] ?? "pm-frontend",
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
                    ClockSkew = TimeSpan.Zero
                };
            });

            builder.Services.AddAuthorization();

            // Enable CORS for Angular dev server
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAngularDev", policy =>
                {
                    policy.WithOrigins("http://localhost:4200")
                          .AllowAnyHeader()
                          .AllowAnyMethod()
                          .AllowCredentials();
                });
            });

            // Dependency injection
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<IProjectService, ProjectCommandService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseCors("AllowAngularDev");

            // **Add Authentication middleware before Authorization**
            app.UseAuthentication();
            app.UseAuthorization();

            // Map Web API controllers
            app.MapControllers();

            // Map Razor Pages
            app.MapRazorPages();

            // Redirect root to Angular login
            app.MapGet("/", context =>
            {
                context.Response.Redirect("http://localhost:4200/login");
                return Task.CompletedTask;
            });

            app.Run();
        }
    }
}
