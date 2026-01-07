using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using pm_backend.DTOs;
using pm_backend.Models;
using pm_backend.Services.Commands.Contracts;
using pm_backend.Services.Queries.Contracts;
using System.Security.Claims;

namespace pm_backend.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        private readonly IUserQueryService _userQueryService;
        private readonly IUserService _userService;

        public UserController(
            IUserQueryService userQueryService,
            IUserService userService
        )
        {
            _userQueryService = userQueryService;
            _userService = userService;
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

        [HttpPut("{id}/isDeleted")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(UserIsDeletedRequest), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateIsDeleted(int id, UserIsDeletedRequest userIsDeleted)
        {
            if (id <= 0)
                return BadRequest("Invalid project id.");

            try
            {

                await _userService.UpdateIsDeletedAsync(id, userIsDeleted.IsDeleted);

                return Ok(NoContent());
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
                    "An unexpected error occurred while updating the user.");
            }
        }
    }
}
