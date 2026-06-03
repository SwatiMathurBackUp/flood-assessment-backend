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
    public class AssessmentController : ControllerBase
    {
        private readonly AssessmentService _assessmentService;

        public AssessmentController(AssessmentService assessmentService)
        {
            _assessmentService = assessmentService;
        }

        // POST api/assessment/sync
        [HttpPost("sync")]
        public async Task<IActionResult> SyncAssessment([FromForm] AssessmentSyncDto dto)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var result = await _assessmentService.SyncAssessmentAsync(dto, userId);
            return Ok(result);
        }

        // GET api/assessment
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var role = User.FindFirst(ClaimTypes.Role)!.Value;
            var assessments = await _assessmentService.GetAllAssessmentsAsync(userId, role);
            return Ok(assessments);
        }

        // GET api/assessment/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var assessment = await _assessmentService.GetAssessmentByIdAsync(id);
            if (assessment == null)
                return NotFound(new { message = $"Assessment {id} not found" });
            return Ok(assessment);
        }

        // DELETE api/assessment/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _assessmentService.DeleteAssessmentAsync(id);
            if (!success)
                return NotFound(new { message = $"Assessment {id} not found" });
            return Ok(new { message = "Assessment deleted successfully" });
        }

        // GET api/assessment/stats
        [HttpGet("stats")]
        public async Task<IActionResult> GetStats()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var role = User.FindFirst(ClaimTypes.Role)!.Value;
            var stats = await _assessmentService.GetStatsAsync(userId, role);
            return Ok(stats);
        }

        // GET api/assessment/report
        [HttpGet("report")]
        public async Task<IActionResult> GetReport()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var role = User.FindFirst(ClaimTypes.Role)!.Value;
            var assessments = await _assessmentService.GetAllAssessmentsAsync(userId, role);
            return Ok(assessments);
        }
    }
}