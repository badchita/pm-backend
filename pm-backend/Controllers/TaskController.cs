using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using pm_backend.DTOs;
using pm_backend.Models;
using pm_backend.Services.Commands.Contracts;
using pm_backend.Services.Queries.Contracts;
using System.Security.Claims;
using System.Threading.Tasks;

namespace pm_backend.Controllers
{
    [ApiController]
    [Route("api/tasks")]
    public class TaskController : ControllerBase
    {
        private readonly ITaskCommentService _taskCommentCommandService;
        private readonly ITaskCommentQueryService _taskCommentQueryService;
        private readonly ITaskStateHistoryQueryService _taskStateHistoryQueryService;
        private readonly ITaskCommentReactionService _taskCommentReactionService;
        private readonly IProjectTaskService _projectTaskService;

        public TaskController(
            ITaskCommentService taskCommentCommandService,
            ITaskCommentQueryService taskCommentQueryService,
            ITaskStateHistoryQueryService taskStateHistoryQueryService,
            ITaskCommentReactionService taskCommentReactionService,
            IProjectTaskService projectTaskService
        )
        {
            _taskCommentCommandService = taskCommentCommandService;
            _taskCommentQueryService = taskCommentQueryService;
            _taskStateHistoryQueryService = taskStateHistoryQueryService;
            _taskCommentReactionService = taskCommentReactionService;
            _projectTaskService = projectTaskService;
        }

        [HttpPost("{taskId}/comments")]
        [Authorize]
        [ProducesResponseType(typeof(TaskComment), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateTaskComment(int taskId, TaskCommentDTO taskCommentRequest)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (taskId <= 0)
                return BadRequest("Invalid taskId id.");

            try
            {

                var taskComment = await _taskCommentCommandService.CreateTaskCommentAsync(taskCommentRequest, taskId);

                return Ok(NoContent());
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

        [HttpGet("{taskId}/comments")]
        [Authorize]
        [ProducesResponseType(typeof(TaskComment), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllTaskComments (int taskId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (taskId <= 0)
                return BadRequest("Invalid taskId id.");

            try
            {

                var result = await _taskCommentQueryService.GetAllTaskCommentsAsync(taskId);

                return Ok(result);
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

        [HttpGet("{taskId}/histories")]
        [Authorize]
        [ProducesResponseType(typeof(TaskComment), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllTaskStateHistory(int taskId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (taskId <= 0)
                return BadRequest("Invalid taskId id.");

            try
            {

                var result = await _taskStateHistoryQueryService.GetAllTaskStateHistoryAsync(taskId);

                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "An unexpected error occurred while getting task history.");
            }
        }

        [HttpPut("{taskId}/comments/{taskCommentId}/reactions")]
        [Authorize]
        [ProducesResponseType(typeof(TaskComment), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateTaskCommentReaction(int taskId, int taskCommentId, TaskCommentReactionRequest taskCommentReactionRequest)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (taskId <= 0)
                return BadRequest("Invalid taskId id.");

            try
            {

                await _taskCommentReactionService.UpdateTaskCommentReactionAsync(taskCommentReactionRequest, taskId, taskCommentId);

                return Ok(NoContent());
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

        [HttpPut("{taskId}/state")]
        [Authorize]
        [ProducesResponseType(typeof(TaskComment), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateTaskState(int taskId, UpdateTaskStateRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (taskId <= 0)
                return BadRequest("Invalid task id.");

            try
            {
                var userEmail = User.Claims
                    .FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value
                    ?? "system@local";

                await _projectTaskService.UpdateTaskStateAsync(taskId, request.State, userEmail);

                return NoContent();
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
