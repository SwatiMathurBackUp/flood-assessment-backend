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

        // // PUT api/farm/{id}/status
        // [HttpPut("{id}/status")]
        // public async Task<IActionResult> UpdateStatus(int id, [FromBody] UpdateFarmStatusDto dto)
        // {
        //     var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        //     var success = await _farmService.UpdateStatusAsync(id, dto.Status, userId);

        //     if (!success)
        //         return NotFound(new { message = "Farm not found" });

        //     return Ok(new { message = "Status updated successfully" });
        // }

        // GET api/farm/map
        [HttpGet("map")]
        public async Task<IActionResult> GetMapPins()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var role = User.FindFirst(ClaimTypes.Role)!.Value;
            var pins = await _farmService.GetMapPinsAsync(userId, role);
            return Ok(pins);
        }

        [HttpPost("create")]
    [Authorize(Roles = "Manager")]
    public async Task<IActionResult> CreateFarm(
        CreateFarmDto dto)
    {
        var farm = await _farmService.CreateFarmAsync(dto);

        return CreatedAtAction(
            nameof(GetFarmById),
            new { id = farm.Id },
            farm);
    }

    [HttpPut("{id}/assign")]
[Authorize(Roles = "Manager")]
public async Task<IActionResult> AssignFarm(
    int id,
    AssignFarmDto dto)
{
    try
    {
        var farm = await _farmService.AssignFarmAsync(id, dto.AssessorUserId);
        return Ok(farm);  // ✅ Now returns complete FarmAssignmentResponseDto
    }
    catch (KeyNotFoundException ex)
    {
        return NotFound(ex.Message);
    }
    catch (InvalidOperationException ex)
    {
        return BadRequest(ex.Message);
    }
}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetFarmById(int id)
    {
        var farm = await _farmService.GetFarmByIdAsync(id);

        if (farm == null)
            return NotFound();

        return Ok(farm);
    }
    [HttpPut("{id}/status")]
    [Authorize(Roles = "Manager,Assessor")]
    public async Task<IActionResult> UpdateFarmStatus(int id, [FromBody] UpdateStatusDto dto)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var farm = await _farmService.UpdateStatusAsync(id, dto.Status, userId);

        if (farm == null)
            return NotFound(new { message = "Farm not found or not assigned to the current user." });

        return Ok(farm);
    }
    }
}