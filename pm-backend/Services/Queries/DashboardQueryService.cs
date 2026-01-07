using Microsoft.EntityFrameworkCore;
using pm_backend.Data;
using pm_backend.DTOs;
using pm_backend.Models;
using pm_backend.Services.Queries.Contracts;

namespace pm_backend.Services.Queries
{
    public class DashboardQueryService : IDashboardQueryService
    {

        private readonly PmDbContext _context;

        public DashboardQueryService(PmDbContext context)
        {
            _context = context;
        }

        public async Task<DashboardDTO> GetDashboardAsync(string userEmail)
        {
            var now = DateTime.UtcNow;
            var upcomingRange = now.AddDays(14);

            var activeProjects = await _context.Projects
                .Where(p =>
                    p.IsDeleted == "N" &&
                    p.IsPublished == "Y" &&
                    p.CreatedBy == userEmail)
                .Select(p => new ActiveProjectProgressDto
                {
                    ProjectId = p.Id,
                    ProjectName = p.ProjectName,
                    DueDate = p.DueDate,

                    TotalTasks = p.Tasks.Count(),
                    ClosedTasks = p.Tasks.Count(t => t.State == TaskState.Closed),

                    ProgressPercentage =
                        p.Tasks.Count() == 0
                            ? 0
                            : (int)Math.Round(
                                (double)p.Tasks.Count(t => t.State == TaskState.Closed) * 100
                                / p.Tasks.Count()
                              )
                })
                .OrderByDescending(p => p.ProgressPercentage)
                .ToListAsync();

            var upcomingDeadlines = await _context.Projects
                .Where(p =>
                    p.CreatedBy == userEmail &&
                    p.IsDeleted == "N" &&
                    p.IsPublished == "Y" &&
                    p.DueDate != null &&
                    p.DueDate <= upcomingRange)
                .OrderBy(p => p.DueDate)
                .Select(p => new UpcomingProjectDto
                {
                    ProjectId = p.Id,
                    ProjectName = p.ProjectName,
                    DueDate = p.DueDate,
                    RemainingDays = EF.Functions.DateDiffDay(now, p.DueDate!.Value)
                })
                .ToListAsync();

            var tasksCompletedByProject = await _context.ProjectTasks
                .Where(t =>
                    t.State == TaskState.Closed &&
                    t.Project.CreatedBy == userEmail)
                .GroupBy(t => t.Project.ProjectName)
                .Select(g => new TasksCompletedByProjectDto
                {
                    ProjectName = g.Key,
                    CompletedTasks = g.Count()
                })
                .OrderByDescending(x => x.CompletedTasks)
                .ToListAsync();

            return new DashboardDTO
            {
                ActiveProjectsCount = activeProjects.Count,
                ActiveProjects = activeProjects,
                UpcomingDeadlines = upcomingDeadlines,
                TasksCompletedByProject = tasksCompletedByProject
            };
        }
    }
}
