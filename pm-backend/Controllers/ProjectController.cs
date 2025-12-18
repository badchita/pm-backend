using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using pm_backend.Data;
using pm_backend.DTOs;
using pm_backend.DTOs.Tasks;
using pm_backend.Models;
using pm_backend.Services.Commands.Contracts;
using pm_backend.Services.Queries.Contracts;
using System.Security.Claims;

namespace pm_backend.Controllers
{
    [ApiController]
    [Route("api/projects")]
    public class ProjectController : ControllerBase
    {
        private readonly PmDbContext _context;
        private readonly IProjectService _projectCommandService;
        private readonly IProjectQueryService _projectQueryService;
        private readonly IProjectTaskService _projectTaskCommandService;
        private readonly IProjectTaskQueryService _projectTaskQueryService;

        public ProjectController(
            PmDbContext context,
            IProjectService projectService,
            IProjectQueryService projectQueryService,
            IProjectTaskService projectTaskService,
            IProjectTaskQueryService projectTaskQueryService
         )
        {
            _context = context;
            _projectCommandService = projectService;
            _projectQueryService = projectQueryService;
            _projectTaskCommandService = projectTaskService;
            _projectTaskQueryService = projectTaskQueryService;
        }

        [HttpPost]
        [Authorize]
        [ProducesResponseType(typeof(Project), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateProject(CreateProjectRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var userEmail = User.Claims
                    .FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value
                    ?? "system@local";

                var project = await _projectCommandService.CreateProject(request, userEmail);

                return CreatedAtAction(nameof(GetProjectById),
                    new { id = project.Id }, project);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "An unexpected error occurred while creating the project.");
            }
        }

        [HttpGet("{id}")]
        [Authorize]
        [ProducesResponseType(typeof(Project), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetProjectById(int id)
        {
            if (id <= 0)
                return BadRequest("Invalid project id.");

            try
            {
                var userEmail = User.Claims
                    .FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value
                    ?? "system@local";

                var project = await _projectCommandService.GetProjectByIdAsync(id, userEmail);

                if (project == null)
                    return NotFound($"Project with id {id} was not found.");

                return Ok(project);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "An unexpected error occurred while retrieving the project.");
            }
        }

        [HttpPut("{id}")]
        [Authorize]
        [ProducesResponseType(typeof(Project), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateProject(int id, UpdateProjectRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (id <= 0)
                return BadRequest("Invalid project id.");

            try
            {
                var userEmail = User.Claims
                    .FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value
                    ?? "system@local";

                var updatedProject = await _projectCommandService.UpdateProjectAsync(id, request, userEmail);

                if (updatedProject == null)
                    return NotFound($"Project with id {id} was not found.");

                return Ok(updatedProject);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "An unexpected error occurred while updating the project.");
            }
        }

        [HttpGet]
        [Authorize]
        [ProducesResponseType(typeof(PagedResult<Project>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetProjects([FromQuery] ProjectListQuery query)
        {
            try
            {
                var userEmail = User.Claims
                    .FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value
                    ?? "system@local";

                var result = await _projectQueryService.GetProjectsAsync(query, userEmail);

                return Ok(result);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "An unexpected error occurred while retrieving projects.");
            }
        }

        [HttpPut("{id}/publish")]
        [Authorize]
        [ProducesResponseType(typeof(Project), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PublishProject(int id, UpdateProjectRequest request)
        {
            if (id <= 0)
                return BadRequest("Invalid project id.");

            try
            {
                var userEmail = User.Claims
                    .FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value
                    ?? "system@local";

                var publishedProject = await _projectCommandService.PublishProjectAsync(id, request, userEmail);

                return Ok(publishedProject);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "An unexpected error occurred while publishing the project.");
            }
        }

        [HttpPut("{id}/unpublish")]
        [Authorize]
        [ProducesResponseType(typeof(Project), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UnPublishProject(int id)
        {
            if (id <= 0)
                return BadRequest("Invalid project id.");

            try
            {
                var userEmail = User.Claims
                    .FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value
                    ?? "system@local";

                var publishedProject = await _projectCommandService.UnPublishProjectAsync(id, userEmail);

                return Ok(publishedProject);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "An unexpected error occurred while publishing the project.");
            }
        }

        [HttpPost("{projectId}/tasks")]
        [Authorize]
        [ProducesResponseType(typeof(ProjectTask), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> CreateTask(int projectId, CreateProjectTaskRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (projectId <= 0)
                return BadRequest("Invalid project id.");

            try
            {
                var userEmail = User.Claims
                    .FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value
                    ?? "system@local";

                request.ProjectId = projectId;

                var task = await _projectTaskCommandService.CreateTaskAsync(request, userEmail);

                return Created($"/api/projects/{projectId}/tasks/{task.Id}", task);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "An unexpected error occurred while creating the task.");
            }
        }

        [HttpGet("{projectId}/tasks")]
        [Authorize]
        [ProducesResponseType(typeof(PagedResult<ProjectTask>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetProjectTasks(int projectId,[FromQuery] ProjectTaskListQuery query)
        {
            if (projectId <= 0)
                return BadRequest("Invalid project id.");

            try
            {
                var userEmail = User.Claims
                    .FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value
                    ?? "system@local";

                var result = await _projectTaskQueryService
                    .GetProjectTasksAsync(projectId, query, userEmail);

                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                     ex.Message
                );
            }
        }

        [HttpGet("{projectId}/tasks/{id}")]
        [Authorize]
        [ProducesResponseType(typeof(Project), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetProjectTaskById(int projectId, int id)
        {
            if (projectId <= 0)
                return BadRequest("Invalid project id.");

            if (id <= 0)
                return BadRequest("Invalid task id.");

            try
            {
                var userEmail = User.Claims
                    .FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value
                    ?? "system@local";

                var task = await _projectTaskCommandService.GetTaskByIdAsync(id, projectId, userEmail);

                if (task == null)
                    return NotFound($"Task with id {id} was not found.");

                return Ok(task);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "An unexpected error occurred while retrieving the project.");
            }
        }

        [HttpPut("{projectId}/tasks/{id}")]
        [Authorize]
        [ProducesResponseType(typeof(Project), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateProjectTask(int id, int projectId, CreateProjectTaskRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (projectId <= 0)
                return BadRequest("Invalid project id.");

            if (id <= 0)
                return BadRequest("Invalid task id.");

            try
            {
                var userEmail = User.Claims
                    .FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value
                    ?? "system@local";

                var updatedProjectTask = await _projectTaskCommandService.UpdateTaskAsync(id, projectId, request, userEmail);

                if (updatedProjectTask == null)
                    return NotFound($"Task with id {id} was not found.");

                return Ok(updatedProjectTask);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "An unexpected error occurred while updating the project.");
            }
        }
    }
}
