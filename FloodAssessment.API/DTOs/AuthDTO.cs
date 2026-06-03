namespace FloodAssessment.API.DTOs
{
    public class LoginRequestDto
    {
        public string Name { get; set; } = string.Empty;
        public string Pin { get; set; } = string.Empty;
    }

    public class LoginResponseDto
    {
        public string Token { get; set; } = string.Empty;
        public int UserId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public DateTime ExpiresAt { get; set; }
    }
}