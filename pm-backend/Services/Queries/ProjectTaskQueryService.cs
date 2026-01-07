using Microsoft.EntityFrameworkCore;
using pm_backend.Data;
using pm_backend.DTOs;
using pm_backend.Models;
using pm_backend.Services.Queries.Contracts;

namespace pm_backend.Services.Queries
{
    public class ProjectTaskQueryService : IProjectTaskQueryService
    {
        private readonly PmDbContext _context;

        public ProjectTaskQueryService(PmDbContext context)
        {
            _context = context;
        }

        public async Task<PagedResult<ProjectTask>> GetProjectTasksAsync(int projectId,ProjectTaskListQuery query,string userEmail)
        {
            var projectExists = await _context.Projects
                .AnyAsync(p => p.Id == projectId);

            if (!projectExists)
                throw new KeyNotFoundException($"Project with id {projectId} not found.");

            IQueryable<ProjectTask> tasks = _context.ProjectTasks
                .Where(t => t.ProjectId == projectId);

            if (!string.IsNullOrWhiteSpace(query.Search))
            {
                tasks = tasks.Where(t =>
                    t.TaskName.Contains(query.Search) ||
                    t.TaskIdNumber.Contains(query.Search) ||
                    t.CreatedBy.Contains(query.Search)
                );
            }

            if (!string.IsNullOrWhiteSpace(query.State))
            {
                if (Enum.TryParse<TaskState>(query.State, ignoreCase: true, out var stateFilter))
                {
                    tasks = tasks.Where(p => p.State == stateFilter);
                }
            }

            if (!string.IsNullOrWhiteSpace(query.AssignedTo))
            {
                tasks = tasks.Where(p => p.AssignedTo == query.AssignedTo);
            }

            var sortBy = query.SortBy?.Trim().ToLower();
            var sortDirection = query.SortDirection?.Trim().ToLower() == "asc" ? "asc" : "desc";

            tasks = sortBy switch
            {
                "taskname" => sortDirection == "asc"
                        ? tasks.OrderBy(p => p.TaskName).ThenBy(p => p.TaskSequence)
                        : tasks.OrderByDescending(p => p.TaskName).ThenByDescending(p => p.TaskSequence),

                "taskidnumber" => sortDirection == "asc"
                    ? tasks.OrderBy(p => p.TaskSequence)
                    : tasks.OrderByDescending(p => p.TaskSequence),

                "createdat" => sortDirection == "asc"
                    ? tasks.OrderBy(p => p.CreatedAt).ThenBy(p => p.TaskSequence)
                    : tasks.OrderByDescending(p => p.CreatedAt).ThenByDescending(p => p.TaskSequence),

                _ => tasks.OrderByDescending(p => p.TaskSequence)
            };

            var totalCount = await tasks.CountAsync();

            var data = await tasks
                .Skip((query.Page - 1) * query.PageSize)
                .Take(query.PageSize)
                .ToListAsync();

            return new PagedResult<ProjectTask>
            {
                Data = data,
                TotalCount = totalCount,
                Page = query.Page,
                PageSize = query.PageSize
            };
        }
    }
}
