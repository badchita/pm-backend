using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using pm_backend.DTOs;
using pm_backend.Models;
using pm_backend.Services.Commands.Contracts;
using pm_backend.Services.Queries.Contracts;

namespace pm_backend.Controllers
{
    [ApiController]
    [Route("api/tasks")]
    public class TaskController : ControllerBase
    {
        private readonly ITaskCommentService _taskCommentCommandService;
        private readonly ITaskCommentQueryService _taskCommentQueryService;

        public TaskController(
            ITaskCommentService taskCommentCommandService,
            ITaskCommentQueryService taskCommentQueryService
        )
        {
            _taskCommentCommandService = taskCommentCommandService;
            _taskCommentQueryService = taskCommentQueryService;
        }

        [HttpPost("{taskId}/comments")]
        [Authorize]
        [ProducesResponseType(typeof(TaskComment), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateTaskComment(int taskId, CreateTaskCommentRequest taskCommentRequest)
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
    }
}
