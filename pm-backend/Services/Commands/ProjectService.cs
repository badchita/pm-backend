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
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == userEmail);

            if (user == null)
                throw new Exception("User not found.");

            var project = new Project
            {
                ProjectName = request.ProjectName,
                Description = request.Description,
                CreatedBy = userEmail,
                ProjectIdNumber = await GenerateProjectNumber(user.CompanyId.Value),
                IsPublished = "N",
                IsDeleted = "N",
                CompanyId = user.CompanyId.Value,
                CreatedAt = DateTime.UtcNow
            };

            _context.Projects.Add(project);
            await _context.SaveChangesAsync();

            return project;
        }

        public async Task<Project?> GetProjectByIdAsync(int id, string userEmail)
        {
            if (string.IsNullOrWhiteSpace(userEmail))
                throw new UnauthorizedAccessException("User not authorized.");

            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == userEmail);

            if (user == null)
                throw new Exception("User not found.");

            var project = await _context.Projects
                .FirstOrDefaultAsync(p => p.Id == id && p.IsDeleted == "N" && p.CompanyId == user.CompanyId.Value);

            return project;
        }

        public async Task<Project?> UpdateProjectAsync(int id, UpdateProjectRequest request, string userEmail)
        {
            if (string.IsNullOrWhiteSpace(userEmail))
                throw new UnauthorizedAccessException("User not authorized.");


            var project = await _context.Projects
                .FirstOrDefaultAsync(p => p.Id == id && p.IsDeleted == "N");

            if (project == null)
                return null;

            project.ProjectName = request.ProjectName;
            project.Description = request.Description;
            project.DueDate = request.DueDate;

            await _context.SaveChangesAsync();

            return project;
        }

        public async Task<Project> PublishProjectAsync(int id, UpdateProjectRequest request, string userEmail)
        {
            if (string.IsNullOrWhiteSpace(userEmail))
                throw new UnauthorizedAccessException("User not authorized.");

            var project = await _context.Projects.FirstOrDefaultAsync(x => x.Id == id && x.IsDeleted == "N");

            if (project == null)
                return null;

            project.ProjectName = request.ProjectName;
            project.Description = request.Description;
            project.DueDate = request.DueDate;
            project.IsPublished = "Y";

            _context.Projects.Update(project);
            await _context.SaveChangesAsync();

            return project;
        }

        public async Task<Project> UnPublishProjectAsync(int id, string userEmail)
        {
            var project = await _context.Projects.FindAsync(id);

            project.IsPublished = "N";

            _context.Projects.Update(project);
            await _context.SaveChangesAsync();

            return project;
        }

        public async Task UpdateIsDeletedAsync(int id, string isDeleted)
        {
            var project = await _context.Projects.FindAsync(id);

            if (project == null)
                throw new KeyNotFoundException("Project not found.");

            project.IsDeleted= isDeleted;

            if (isDeleted == "Y")
            {
                project.IsPublished = "N";
            }

            _context.Projects.Update(project);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteProjectAsync(int projectId)
        {
            var project = await _context.Projects
                .Include(p => p.Tasks)
                    .ThenInclude(t => t.Comments)
                        .ThenInclude(c => c.Reactions)
                .Include(p => p.Tasks)
                    .ThenInclude(t => t.StateHistories)
                .FirstOrDefaultAsync(p => p.Id == projectId);

            if (project == null)
                throw new KeyNotFoundException("Project not found.");

            foreach (var task in project.Tasks)
            {
                foreach (var comment in task.Comments)
                {
                    _context.TaskCommentReactions.RemoveRange(comment.Reactions);
                }

                _context.TaskComments.RemoveRange(task.Comments);

                _context.TaskStateHistories.RemoveRange(task.StateHistories);
            }

            _context.ProjectTasks.RemoveRange(project.Tasks);
            _context.Projects.Remove(project);

            await _context.SaveChangesAsync();
        }

        private async Task<string> GenerateProjectNumber(int companyId)
        {
            var latestProject = await _context.Projects
                .Where(p => p.CompanyId == companyId)
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
