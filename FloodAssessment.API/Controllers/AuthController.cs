using FloodAssessment.API.DTOs;
using FloodAssessment.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace FloodAssessment.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        // POST api/auth/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto dto)
        {
            if (string.IsNullOrEmpty(dto.Name) || string.IsNullOrEmpty(dto.Pin))
                return BadRequest(new { message = "Name and PIN are required" });;

            var result = await _authService.LoginAsync(dto);

            if (result == null)
                return Unauthorized(new { message = "Invalid name or PIN" });

            return Ok(result);
        }
    }
}