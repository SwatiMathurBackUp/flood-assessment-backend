using FloodAssessment.API.DTOs;
using FloodAssessment.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FloodAssessment.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class FarmController : ControllerBase
    {
        private readonly FarmService _farmService;

        public FarmController(FarmService farmService)
        {
            _farmService = farmService;
        }

        // GET api/farm/my
        [HttpGet("my")]
        public async Task<IActionResult> GetMyFarms()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var farms = await _farmService.GetAssignedFarmsAsync(userId);
            return Ok(farms);
        }

        // GET api/farm/all (Manager only)
        [HttpGet("all")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> GetAllFarms()
        {
            var farms = await _farmService.GetAllFarmsAsync();
            return Ok(farms);
        }

        // PUT api/farm/{id}/status
        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] UpdateFarmStatusDto dto)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var success = await _farmService.UpdateStatusAsync(id, dto.Status, userId);

            if (!success)
                return NotFound(new { message = "Farm not found" });

            return Ok(new { message = "Status updated successfully" });
        }

        // GET api/farm/map
        [HttpGet("map")]
        public async Task<IActionResult> GetMapPins()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var role = User.FindFirst(ClaimTypes.Role)!.Value;
            var pins = await _farmService.GetMapPinsAsync(userId, role);
            return Ok(pins);
        }
    }
}