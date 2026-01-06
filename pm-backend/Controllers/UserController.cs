using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using pm_backend.DTOs;
using pm_backend.Models;
using pm_backend.Services.Queries.Contracts;
using System.Security.Claims;

namespace pm_backend.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        private readonly IUserQueryService _userQueryService;

        public UserController(
            IUserQueryService userQueryService
        )
        {
            _userQueryService = userQueryService;
        }

        [HttpGet("search")]
        [Authorize]
        [ProducesResponseType(typeof(User), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllSearchUser([FromQuery] string? search)
        {
            try
            {

                var result = await _userQueryService.GetAllUsersAsync(search);

                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "An unexpected error occurred while getting users.");
            }
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(PagedResult<User>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetUsers([FromQuery] UserListQuery query)
        {
            try
            {
                var userEmail = User.Claims
                    .FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value
                    ?? "system@local";

                var result = await _userQueryService.GetUsersAsync(query, userEmail);

                return Ok(result);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "An unexpected error occurred while retrieving users.");
            }
        }
    }
}
