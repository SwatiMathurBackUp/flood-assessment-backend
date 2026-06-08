using FloodAssessment.API.Data;
using FloodAssessment.API.DTOs;
using Microsoft.EntityFrameworkCore;

namespace FloodAssessment.API.Services
{
    public class UserService
    {
        private readonly AppDbContext _context;

        public UserService(AppDbContext context)
        {
            _context = context;
        }

        // Get all assessors for dropdown/assignment
        public async Task<List<AssessorDto>> GetAssessorsAsync()
        {
            var assessors = await _context.Users
                .Where(u => u.Role == "Assessor")
                .Select(u => new AssessorDto
                {
                    Id = u.Id,
                    Name = u.Name
                })
                .ToListAsync();

            return assessors;
        }
    }
}