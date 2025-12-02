using Microsoft.EntityFrameworkCore;
using pm_backend.Data;
using pm_backend.DTOs;
using pm_backend.Models;
using pm_backend.Services.Commands.Contracts;

namespace pm_backend.Services.Commands
{
    public class ProjectCommandService : IProjectService
    {
        private readonly PmDbContext _context;

        public ProjectCommandService(PmDbContext context)
        {
            _context = context;
        }

        public async Task<Project> CreateProject(CreateProjectRequest request, string userEmail)
        {
            var project = new Project
            {
                ProjectName = request.ProjectName,
                Description = request.Description,
                CreatedBy = userEmail,
                ProjectIdNumber = await GenerateProjectNumber(),
                IsPublished = "N",
                IsDeleted = "N",
                CreatedAt = DateTime.UtcNow
            };

            _context.Projects.Add(project);
            await _context.SaveChangesAsync();

            return project;
        }

        private async Task<string> GenerateProjectNumber()
        {
            var latestProject = await _context.Projects
                .OrderByDescending(p => p.Id)
                .FirstOrDefaultAsync();

            int next = 1;

            if (latestProject != null &&
                !string.IsNullOrEmpty(latestProject.ProjectIdNumber) &&
                latestProject.ProjectIdNumber.Contains('-'))
            {
                var lastNumber = latestProject.ProjectIdNumber.Split('-').Last();

                if (int.TryParse(lastNumber, out int current))
                {
                    next = current + 1;
                }
            }

            return $"PRJ-{next.ToString("D4")}";
        }
    }
}
