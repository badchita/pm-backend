using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using pm_backend.DTOs;
using pm_backend.Models;
using pm_backend.Services.Queries.Contracts;
using System.Security.Claims;

namespace pm_backend.Controllers
{
    [ApiController]
    [Route("api/dashboards")]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardQueryService _dashboardQueryService;

        public DashboardController(
            IDashboardQueryService dashboardQueryService
        )
        {
            _dashboardQueryService = dashboardQueryService;
        }

        [HttpGet]
        [Authorize]
        [ProducesResponseType(typeof(PagedResult<Project>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<DashboardDTO>> GetDashboard()
        {
            try
            {
                var userEmail = User.Claims
                     .FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value
                     ?? "system@local";

                var result = await _dashboardQueryService.GetDashboardAsync(userEmail);
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
    }
}
