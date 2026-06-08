using FloodAssessment.API.Data;
using FloodAssessment.API.DTOs;
using Microsoft.EntityFrameworkCore;
using FloodAssessment.API.Models; 

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
                AssignedToUserName  = f.AssignedTo.Name
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
                AssignedToUserId = f.AssignedToUserId,  
                AssignedToUserName  = f.AssignedTo.Name
            }).ToList();
        }

        // Update farm status
        public async Task<FarmAssignmentResponseDto?> UpdateStatusAsync(int farmId, string status, int userId)
        {
            var farm = await _context.FarmAssignments
                .FirstOrDefaultAsync(f => f.Id == farmId && f.AssignedToUserId == userId);

            if (farm == null)
                return null;

            farm.Status = status;
            if (status == FarmStatus.Completed)
                farm.CompletedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            var result = new FarmAssignmentResponseDto
            {
                Id = farm.Id,
                FarmName = farm.FarmName,
                OwnerName = farm.OwnerName,
                Address = farm.Address,
                Latitude = farm.Latitude,
                Longitude = farm.Longitude,
                EstimatedChickens = farm.EstimatedChickens,
                Status = farm.Status,
                CreatedAt = farm.CreatedAt,
                CompletedAt = farm.CompletedAt,
                AssignedToUserId = farm.AssignedToUserId,
                AssignedToUserName = farm.AssignedTo?.Name
            };
            return result;
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

        public async Task<FarmDto> CreateFarmAsync(CreateFarmDto dto)
{
    var farm = new FarmAssignment  // ✅ FIXED: Create entity, not DTO
    {
        FarmName = dto.FarmName,
        OwnerName = dto.OwnerName,
        Address = dto.Address,
        Latitude = dto.Latitude,
        Longitude = dto.Longitude,
        EstimatedChickens = dto.EstimatedChickens,
        Status = FarmStatus.Pending,
        CreatedAt = DateTime.UtcNow
    };

    _context.FarmAssignments.Add(farm);
    await _context.SaveChangesAsync();

    return new FarmDto
    {
        Id = farm.Id,
        FarmName = farm.FarmName,
        OwnerName = farm.OwnerName,
        Address = farm.Address,
        EstimatedChickens = farm.EstimatedChickens,
        Status = farm.Status,
        CreatedAt = farm.CreatedAt
    };
}

    public async Task<FarmAssignmentResponseDto> AssignFarmAsync(
    int farmId,
    int assessorUserId)
{
    var farm = await _context.FarmAssignments
        .Include(f => f.AssignedTo)
        .FirstOrDefaultAsync(x => x.Id == farmId);

    if (farm == null)
        throw new KeyNotFoundException("Farm not found");

    if (farm.AssignedToUserId.HasValue && farm.AssignedToUserId.Value != 0)
        throw new InvalidOperationException("Farm is already assigned and cannot be changed");

    var assessor = await _context.Users
        .FirstOrDefaultAsync(x => x.Id == assessorUserId);

    if (assessor == null)
        throw new KeyNotFoundException("Assessor not found");

    if (assessor.Role != "Assessor")
        throw new InvalidOperationException("User must be an assessor");

    // ✅ Assign the farm
    farm.AssignedToUserId = assessorUserId;
    farm.Status = FarmStatus.Pending;

    await _context.SaveChangesAsync();

    // ✅ Re-fetch the farm to ensure all data is loaded
    var updatedFarm = await _context.FarmAssignments
        .Include(f => f.AssignedTo)
        .FirstOrDefaultAsync(x => x.Id == farmId);

    return new FarmAssignmentResponseDto
    {
        Id = updatedFarm.Id,
        FarmName = updatedFarm.FarmName,
        OwnerName = updatedFarm.OwnerName,
        Address = updatedFarm.Address,
        Latitude = updatedFarm.Latitude,
        Longitude = updatedFarm.Longitude,
        EstimatedChickens = updatedFarm.EstimatedChickens,
        Status = updatedFarm.Status,
        CreatedAt = updatedFarm.CreatedAt,
        CompletedAt = updatedFarm.CompletedAt,
        AssessmentId = updatedFarm.AssessmentId,
        AssignedToUserName  = updatedFarm.AssignedTo?.Name ?? "Unassigned"
    };
}

   public async Task<FarmDetailsDto?> GetFarmByIdAsync(int id)
{
    return await _context.FarmAssignments
        .Include(x => x.AssignedTo)  // ✅ CHANGED from AssignedToUser
        .Where(x => x.Id == id)
        .Select(x => new FarmDetailsDto
        {
            Id = x.Id,
            FarmName = x.FarmName,
            OwnerName = x.OwnerName,
            Address = x.Address,
            Latitude = x.Latitude,
            Longitude = x.Longitude,
            EstimatedChickens = x.EstimatedChickens,
            Status = x.Status,
            AssignedToUserId = x.AssignedToUserId,
            AssignedToUserName = x.AssignedTo.Name  // ✅ CHANGED from AssignedToUser
        })
        .FirstOrDefaultAsync();
}
    }
}