using Microsoft.EntityFrameworkCore;
using pm_backend.Data;
using pm_backend.Services.Commands;
using pm_backend.Services.Commands.Contracts;

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

            builder.Services.AddDbContext<PmDbContext>(options =>
            {
                options.UseSqlServer(
                   "Server=localhost;Database=pm;Trusted_Connection=True;TrustServerCertificate=True;"
                );
            });

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
