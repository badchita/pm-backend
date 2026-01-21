using Microsoft.EntityFrameworkCore;
using pm_backend.Data;
using pm_backend.DTOs;
using pm_backend.Models;
using pm_backend.Services.Queries.Contracts;
using System.Data;

namespace pm_backend.Services.Queries
{
    public class ProjectQueryService : IProjectQueryService
    {
        private readonly PmDbContext _context;

        public ProjectQueryService(PmDbContext context)
        {
            _context = context;
        }

        public async Task<PagedResult<Project>> GetProjectsAsync(ProjectListQuery query, string userEmail)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == userEmail);

            if (user == null)
                throw new Exception("User not found.");

            IQueryable<Project> projects = _context.Projects
                .Include(p => p.Company)
                .AsQueryable();


            if (user.Role == UserRole.Manager)
            {
                projects = projects.Where(p => p.CompanyId == user.CompanyId);
            }

            if (!string.IsNullOrWhiteSpace(query.Search))
            {
                projects = projects.Where(p =>
                    p.ProjectName.Contains(query.Search) ||
                    p.Description.Contains(query.Search) ||
                    p.ProjectIdNumber.Contains(query.Search) ||
                    p.CreatedBy.Contains(query.Search)
                );
            }

            if (query.DueDate.HasValue)
            {
                var dueDate = query.DueDate.Value.Date;

                projects = projects.Where(p =>
                    p.DueDate.HasValue &&
                    p.DueDate.Value.Date == dueDate
                );
            }

            if (!string.IsNullOrWhiteSpace(query.IsPublished))
            {
                projects = projects.Where(p => p.IsPublished == query.IsPublished);
            }

            if (!string.IsNullOrWhiteSpace(query.IsDeleted))
            {
                projects = projects.Where(p => p.IsDeleted == query.IsDeleted);
            }

            if (query.CompanyId.HasValue)
            {
                projects = projects.Where(p => p.CompanyId == query.CompanyId);
            }

            var sortBy = query.SortBy?.Trim().ToLower();
            var sortDirection = query.SortDirection?.Trim().ToLower() == "asc" ? "asc" : "desc";

            projects = sortBy switch
            {
                "projectname" => sortDirection == "asc"
                    ? projects.OrderBy(p => p.ProjectName)
                    : projects.OrderByDescending(p => p.ProjectName),

                "projectidnumber" => sortDirection == "asc"
                    ? projects.OrderBy(p => p.ProjectIdNumber)
                    : projects.OrderByDescending(p => p.ProjectIdNumber),

                "ispublished" => sortDirection == "asc"
                    ? projects.OrderBy(p => p.IsPublished)
                    : projects.OrderByDescending(p => p.IsPublished),

                "duedate" => sortDirection == "asc"
                    ? projects.OrderBy(p => p.DueDate)
                    : projects.OrderByDescending(p => p.DueDate),

                "createdat" => sortDirection == "asc"
                    ? projects.OrderBy(p => p.CreatedAt)
                    : projects.OrderByDescending(p => p.CreatedAt),

                _ => projects.OrderByDescending(p => p.CreatedAt)
            };

            var totalCount = await projects.CountAsync();

            var data = await projects
                .Skip((query.Page - 1) * query.PageSize)
                .Take(query.PageSize)
                .ToListAsync();

            return new PagedResult<Project>
            {
                Data = data,
                TotalCount = totalCount,
                Page = query.Page,
                PageSize = query.PageSize
            };
        }

        public async Task<ProjectTaskBoardResponse?> GetProjectWithTasksAsync(int projectId)
        {
            var project = await _context.Projects
                .Include(p => p.Tasks)
                .FirstOrDefaultAsync(p => p.Id == projectId);

            if (project == null) return null;

            var response = new ProjectTaskBoardResponse
            {
                Id = project.Id,
                ProjectIdNumber = project.ProjectIdNumber,
                ProjectName = project.ProjectName,
                Tasks = project.Tasks.Select(t => new ProjectTaskDto
                {
                    Id = t.Id,
                    TaskName = t.TaskName,
                    Description = t.Description,
                    AssignedTo = t.AssignedTo,
                    TaskIdNumber = t.TaskIdNumber,
                    State = (TaskState)t.State,
                    CreatedBy = t.CreatedBy,
                    UpdatedBy = t.UpdatedBy,
                    ProjectId = t.ProjectId
                }).ToList()
            };

            return response;
        }
    }
}
