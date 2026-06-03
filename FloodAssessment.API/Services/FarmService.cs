using FloodAssessment.API.Data;
using FloodAssessment.API.DTOs;
using Microsoft.EntityFrameworkCore;

namespace FloodAssessment.API.Services
{
    public class FarmService
    {
        private readonly AppDbContext _context;

        public FarmService(AppDbContext context)
        {
            _context = context;
        }

        // Get farms assigned to a specific user
        public async Task<List<FarmAssignmentResponseDto>> GetAssignedFarmsAsync(int userId)
        {
            var farms = await _context.FarmAssignments
                .Include(f => f.AssignedTo)
                .Include(f => f.Assessment)
                .Where(f => f.AssignedToUserId == userId)
                .OrderBy(f => f.Status)
                .ToListAsync();

            return farms.Select(f => new FarmAssignmentResponseDto
            {
                Id = f.Id,
                FarmName = f.FarmName,
                OwnerName = f.OwnerName,
                Address = f.Address,
                Latitude = f.Latitude,
                Longitude = f.Longitude,
                EstimatedChickens = f.EstimatedChickens,
                Status = f.Status,
                CreatedAt = f.CreatedAt,
                CompletedAt = f.CompletedAt,
                AssessmentId = f.AssessmentId,
                AssignedToName = f.AssignedTo.Name
            }).ToList();
        }

        // Get all farms (manager view)
        public async Task<List<FarmAssignmentResponseDto>> GetAllFarmsAsync()
        {
            var farms = await _context.FarmAssignments
                .Include(f => f.AssignedTo)
                .Include(f => f.Assessment)
                .OrderBy(f => f.Status)
                .ToListAsync();

            return farms.Select(f => new FarmAssignmentResponseDto
            {
                Id = f.Id,
                FarmName = f.FarmName,
                OwnerName = f.OwnerName,
                Address = f.Address,
                Latitude = f.Latitude,
                Longitude = f.Longitude,
                EstimatedChickens = f.EstimatedChickens,
                Status = f.Status,
                CreatedAt = f.CreatedAt,
                CompletedAt = f.CompletedAt,
                AssessmentId = f.AssessmentId,
                AssignedToName = f.AssignedTo.Name
            }).ToList();
        }

        // Update farm status
        public async Task<bool> UpdateStatusAsync(int farmId, string status, int userId)
        {
            var farm = await _context.FarmAssignments
                .FirstOrDefaultAsync(f => f.Id == farmId && f.AssignedToUserId == userId);

            if (farm == null) return false;

            farm.Status = status;
            if (status == "Completed") farm.CompletedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }

        // Get map pins
        public async Task<List<MapPinDto>> GetMapPinsAsync(int userId, string role)
        {
            var query = _context.FarmAssignments
                .Include(f => f.AssignedTo)
                .Include(f => f.Assessment)
                .AsQueryable();

            // Assessors only see their own farms
            if (role != "Manager")
                query = query.Where(f => f.AssignedToUserId == userId);

            var farms = await query.ToListAsync();

            return farms.Select(f => new MapPinDto
            {
                Id = f.Id,
                FarmName = f.FarmName,
                Address = f.Address,
                Latitude = f.Latitude,
                Longitude = f.Longitude,
                Status = f.Status,
                AssignedToName = f.AssignedTo.Name,
                Condition = f.Assessment?.Condition,
                ChickenCount = f.Assessment?.ChickenCount
            }).ToList();
        }
    }
}