using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FloodAssessment.API.Services;

namespace FloodAssessment.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService;
        }

        [HttpGet("assessors")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> GetAssessors()
        {
            try
            {
                var assessors = await _userService.GetAssessorsAsync();
                return Ok(assessors);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error fetching assessors: {ex.Message}");
            }
        }
    }
}
