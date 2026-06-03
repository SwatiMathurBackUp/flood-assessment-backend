using FloodAssessment.API.Data;
using FloodAssessment.API.DTOs;
using FloodAssessment.API.Models;
using Microsoft.EntityFrameworkCore;

namespace FloodAssessment.API.Services
{
    public class AssessmentService
    {
        private readonly AppDbContext _context;
        private readonly CloudinaryService _cloudinaryService;

        public AssessmentService(AppDbContext context, CloudinaryService cloudinaryService)
        {
            _context = context;
            _cloudinaryService = cloudinaryService;
        }

        public async Task<AssessmentResponseDto> SyncAssessmentAsync(AssessmentSyncDto dto, int userId)
        {
            // Check for duplicate
            var existing = await _context.Assessments
                .Include(a => a.Photos)
                .FirstOrDefaultAsync(a => a.ClientId == dto.ClientId);

            if (existing != null)
                return MapToResponseDto(existing);

            // Create assessment
            var assessment = new Assessment
            {
                ClientId = dto.ClientId,
                AssessorName = dto.AssessorName,
                Address = dto.Address,
                Latitude = dto.Latitude,
                Longitude = dto.Longitude,
                Condition = dto.Condition,
                ChickenCount = dto.ChickenCount,
                Notes = dto.Notes,
                UserId = userId,
                CreatedAt = DateTime.TryParse(dto.CreatedAt, out var date)
                    ? date : DateTime.UtcNow,
                SyncedAt = DateTime.UtcNow
            };

            // Upload photos
            if (dto.Photos != null && dto.Photos.Count > 0)
            {
                foreach (var file in dto.Photos)
                {
                    if (file.Length > 0)
                    {
                        var uploadResult = await _cloudinaryService.UploadPhotoAsync(file);
                        assessment.Photos.Add(new Photo
                        {
                            CloudinaryUrl = uploadResult.Url,
                            CloudinaryPublicId = uploadResult.PublicId,
                            Filename = file.FileName,
                            CapturedAt = DateTime.UtcNow
                        });
                    }
                }
            }

            _context.Assessments.Add(assessment);
            await _context.SaveChangesAsync();

            // Update farm assignment status to Completed
            var farm = await _context.FarmAssignments
                .FirstOrDefaultAsync(f => f.AssignedToUserId == userId
                    && f.Status != "Completed"
                    && f.Address == dto.Address);

            if (farm != null)
            {
                farm.Status = "Completed";
                farm.AssessmentId = assessment.Id;
                farm.CompletedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }

            return MapToResponseDto(assessment);
        }

        public async Task<List<AssessmentResponseDto>> GetAllAssessmentsAsync(int userId, string role)
        {
            var query = _context.Assessments
                .Include(a => a.Photos)
                .AsQueryable();

            // Assessors only see their own
            if (role != "Manager")
                query = query.Where(a => a.UserId == userId);

            var assessments = await query
                .OrderByDescending(a => a.CreatedAt)
                .ToListAsync();

            return assessments.Select(MapToResponseDto).ToList();
        }

        public async Task<AssessmentResponseDto?> GetAssessmentByIdAsync(int id)
        {
            var assessment = await _context.Assessments
                .Include(a => a.Photos)
                .FirstOrDefaultAsync(a => a.Id == id);

            return assessment == null ? null : MapToResponseDto(assessment);
        }

        public async Task<bool> DeleteAssessmentAsync(int id)
        {
            var assessment = await _context.Assessments
                .Include(a => a.Photos)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (assessment == null) return false;

            foreach (var photo in assessment.Photos)
                await _cloudinaryService.DeletePhotoAsync(photo.CloudinaryPublicId);

            _context.Assessments.Remove(assessment);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<AssessmentStatsDto> GetStatsAsync(int userId, string role)
        {
            var query = _context.Assessments.AsQueryable();

            if (role != "Manager")
                query = query.Where(a => a.UserId == userId);

            var assessments = await query.ToListAsync();

            // Get farm stats
            var farmQuery = _context.FarmAssignments.AsQueryable();
            if (role != "Manager")
                farmQuery = farmQuery.Where(f => f.AssignedToUserId == userId);

            var farms = await farmQuery.ToListAsync();

            return new AssessmentStatsDto
            {
                Total = assessments.Count,
                Good = assessments.Count(a => a.Condition == "Good"),
                Moderate = assessments.Count(a => a.Condition == "Moderate"),
                Bad = assessments.Count(a => a.Condition == "Bad"),
                TotalChickens = assessments.Sum(a => a.ChickenCount),
                TotalFarms = farms.Count,
                PendingFarms = farms.Count(f => f.Status == "Pending"),
                InProgressFarms = farms.Count(f => f.Status == "InProgress"),
                CompletedFarms = farms.Count(f => f.Status == "Completed")
            };
        }

        private AssessmentResponseDto MapToResponseDto(Assessment assessment)
        {
            return new AssessmentResponseDto
            {
                Id = assessment.Id,
                ClientId = assessment.ClientId,
                AssessorName = assessment.AssessorName,
                Address = assessment.Address,
                Latitude = assessment.Latitude,
                Longitude = assessment.Longitude,
                Condition = assessment.Condition,
                ChickenCount = assessment.ChickenCount,
                Notes = assessment.Notes,
                CreatedAt = assessment.CreatedAt,
                SyncedAt = assessment.SyncedAt,
                Photos = assessment.Photos.Select(p => new PhotoResponseDto
                {
                    Id = p.Id,
                    CloudinaryUrl = p.CloudinaryUrl,
                    Filename = p.Filename,
                    CapturedAt = p.CapturedAt
                }).ToList()
            };
        }
    }
}