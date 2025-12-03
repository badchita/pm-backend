using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using pm_backend.Data;
using pm_backend.DTOs;
using pm_backend.Models;
using pm_backend.Services.Commands.Contracts;
using System.Security.Claims;
using System.Threading.Tasks;

namespace pm_backend.Controllers
{
    [ApiController]
    [Route("api/projects")]
    public class ProjectController : ControllerBase
    {
        private readonly PmDbContext _context;
        private readonly IProjectService _projectCommandService;

        public ProjectController(
            PmDbContext context,
            IProjectService projectService
         )
        {
            _context = context;
            _projectCommandService = projectService;
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
        public async Task<IActionResult> GetProjectById(int id)
        {
            var project = await _context.Projects.FindAsync(id);

            if (project == null)
                return NotFound();

            return Ok(project);
        }
    }
}
