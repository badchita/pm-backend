using Microsoft.EntityFrameworkCore;
using pm_backend.Data;
using pm_backend.DTOs.Tasks;
using pm_backend.Models;
using pm_backend.Services.Commands.Contracts;

namespace pm_backend.Services.Commands
{
    public class ProjectTaskService : IProjectTaskService
    {
        private readonly PmDbContext _context;

        public ProjectTaskService(PmDbContext context)
        {
            _context = context;
        }

        public async Task<ProjectTask> CreateTaskAsync(CreateProjectTaskRequest request, string userEmail)
        {
            var project = await _context.Projects
                .FirstOrDefaultAsync(p => p.Id == request.ProjectId);

            if (project == null)
                throw new KeyNotFoundException("Project not found.");

            var (taskNumber, taskSequence) = await GenerateTaskNumberAsync(request.ProjectId);

            var task = new ProjectTask
            {
                ProjectId = request.ProjectId,
                TaskName = request.TaskName,
                Description = request.Description,
                AcceptanceCriteria = request.AcceptanceCriteria,
                AssignedTo = request.AssignedTo,
                TaskPoints = request.TaskPoints,
                ReadyForDevelopmentDate = request.ReadyForDevelopmentDate,
                DoneDate = request.DoneDate,
                TestingStartDate = request.TestingStartDate,
                TestingEndDate = request.TestingEndDate,
                CreatedBy = userEmail,
                TaskIdNumber = taskNumber,
                TaskSequence = taskSequence,
                CreatedAt = DateTime.UtcNow
            };

            _context.ProjectTasks.Add(task);
            await _context.SaveChangesAsync();

            var history = new TaskStateHistory
            {
                TaskId = task.Id,
                NewState = task.State,
                ChangedBy = userEmail,
                ChangedAt = DateTime.UtcNow
            };
            _context.TaskStateHistories.Add(history);
            await _context.SaveChangesAsync();

            return task;
        }

        public async Task<ProjectTask?> GetTaskByIdAsync(int id, int projectId, string userEmail)
        {
            if (string.IsNullOrWhiteSpace(userEmail))
                throw new UnauthorizedAccessException("User not authorized.");

            var task = await _context.ProjectTasks
                .FirstOrDefaultAsync(t => t.Id == id && t.ProjectId == projectId);

            return task;
        }
        public async Task<ProjectTask?> UpdateTaskAsync(int id, int projectId, CreateProjectTaskRequest request, string userEmail)
        {
            if (string.IsNullOrWhiteSpace(userEmail))
                throw new UnauthorizedAccessException("User not authorized.");

            var task = await _context.ProjectTasks.FirstOrDefaultAsync(t => t.Id == id && t.ProjectId == projectId);

            if (task == null)
                return null;

            var previousState = task.State;

            task.ProjectId = request.ProjectId;
            task.TaskName = request.TaskName;
            task.Description = request.Description;
            task.AcceptanceCriteria = request.AcceptanceCriteria;
            task.AssignedTo = request.AssignedTo;
            task.TaskPoints = request.TaskPoints;
            task.ReadyForDevelopmentDate = request.ReadyForDevelopmentDate;
            task.DoneDate = request.DoneDate;
            task.TestingStartDate = request.TestingStartDate;
            task.TestingEndDate = request.TestingEndDate;
            task.State = (TaskState)request.State;

            if (previousState != task.State)
            {
                var history = new TaskStateHistory
                {
                    TaskId = task.Id,
                    PreviousState = previousState,
                    NewState = task.State,
                    ChangedBy = userEmail,
                    ChangedAt = DateTime.UtcNow
                };
                _context.TaskStateHistories.Add(history);
            }

            await _context.SaveChangesAsync();

            return task;
        }

        private async Task<(string taskNumber, int taskSequence)> GenerateTaskNumberAsync(int projectId)
        {
            var project = await _context.Projects
                .Where(p => p.Id == projectId)
                .Select(p => new { p.ProjectIdNumber })
                .FirstOrDefaultAsync();

            if (project == null)
                throw new KeyNotFoundException("Project not found.");

            var lastSequence = await _context.ProjectTasks
                .Where(t => t.ProjectId == projectId)
                .MaxAsync(t => (int?)t.TaskSequence) ?? 0;

            var nextSequence = lastSequence + 1;

            var taskNumber = $"{project.ProjectIdNumber}-T{nextSequence:D2}";

            return (taskNumber, nextSequence);
        }
    }
}
