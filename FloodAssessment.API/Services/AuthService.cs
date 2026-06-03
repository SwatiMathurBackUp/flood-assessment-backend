using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using FloodAssessment.API.Data;
using FloodAssessment.API.DTOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace FloodAssessment.API.Services
{
    public class AuthService
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthService(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<LoginResponseDto?> LoginAsync(LoginRequestDto dto)
        {
            // Find user by name
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Name.ToLower() == dto.Name.ToLower());

            if (user == null) return null;

            // Verify PIN
            bool validPin = BCrypt.Net.BCrypt.Verify(dto.Pin, user.PinHash);
            if (!validPin) return null;

            // Generate JWT token
            var expiresAt = DateTime.UtcNow.AddHours(8);
            var token = GenerateToken(user.Id, user.Name, user.Role, expiresAt);

            return new LoginResponseDto
            {
                Token = token,
                UserId = user.Id,
                Name = user.Name,
                Role = user.Role,
                ExpiresAt = expiresAt
            };
        }

        private string GenerateToken(int userId, string name, string role, DateTime expiresAt)
        {
            var jwtKey = _configuration["Jwt:Key"] ?? "FloodAssessmentSecretKey2024!";
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                new Claim(ClaimTypes.Name, name),
                new Claim(ClaimTypes.Role, role)
            };

            var token = new JwtSecurityToken(
                issuer: "FloodAssessment",
                audience: "FloodAssessment",
                claims: claims,
                expires: expiresAt,
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}