using Microsoft.EntityFrameworkCore;
using pm_backend.Data;
using pm_backend.DTOs;
using pm_backend.Models;
using pm_backend.Services.Queries.Contracts;
using System.Threading.Tasks;

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
            IQueryable<Project> projects = _context.Projects.AsQueryable();

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

        public async Task<PagedResult<User>> GetCompanyUsersAsync(int companyId, UserListQuery query)
        {
            var companyExists = await _context.Companies
                .AnyAsync(c => c.Id == companyId);

            if (!companyExists)
                throw new KeyNotFoundException($"Project with id {companyId} not found.");

            IQueryable<User> users = _context.Users
                .Where(u => u.CompanyId== companyId);

            if (!string.IsNullOrWhiteSpace(query.Search))
            {
                users = users.Where(u =>
                    u.Name.Contains(query.Search) ||
                    u.Email.Contains(query.Search)
                );
            }

            if (!string.IsNullOrWhiteSpace(query.IsApproved))
            {
                users = users.Where(u => u.IsApproved == query.IsApproved);
            }

            if (!string.IsNullOrWhiteSpace(query.Role))
            {
                if (Enum.TryParse<UserRole>(query.Role, ignoreCase: true, out var roleFilter))
                {
                    users = users.Where(u => u.Role == roleFilter);
                }
            }

            var sortBy = query.SortBy?.Trim().ToLower();
            var sortDirection = query.SortDirection?.Trim().ToLower() == "asc" ? "asc" : "desc";

            users = sortBy switch
            {
                "name" => sortDirection == "asc"
                    ? users.OrderBy(u => u.Name)
                    : users.OrderByDescending(u => u.Name),

                "email" => sortDirection == "asc"
                    ? users.OrderBy(u => u.Email)
                    : users.OrderByDescending(u => u.Email),

                "createdat" => sortDirection == "asc"
                    ? users.OrderBy(u => u.CreatedAt)
                    : users.OrderByDescending(u => u.CreatedAt),

                _ => users.OrderByDescending(u => u.CreatedAt)
            };

            var totalCount = await users.CountAsync();

            var data = await users
                .Skip((query.Page - 1) * query.PageSize)
                .Take(query.PageSize)
                .ToListAsync();

            return new PagedResult<User>
            {
                Data = data,
                TotalCount = totalCount,
                Page = query.Page,
                PageSize = query.PageSize
            };
        }
    }
}
