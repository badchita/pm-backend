using Microsoft.EntityFrameworkCore;
using pm_backend.Data;

namespace pm_backend
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorPages();

            builder.Services.AddDbContext<PmDbContext>(options =>
            {
                options.UseSqlServer(
                   "Server=localhost;Database=pm;Trusted_Connection=True;TrustServerCertificate=True;"
                );

            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapGet("/", context =>
            {
                context.Response.Redirect("http://localhost:4200/login");
                return Task.CompletedTask;
            });

            app.MapRazorPages();

            app.Run();
        }
    }
}
