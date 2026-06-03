namespace FloodAssessment.API.DTOs
{
    public class AssessmentSyncDto
    {
        public string ClientId { get; set; } = string.Empty;
        public string AssessorName { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Condition { get; set; } = string.Empty;
        public int ChickenCount { get; set; }
        public string? Notes { get; set; }
        public string CreatedAt { get; set; } = string.Empty;
        public List<IFormFile>? Photos { get; set; }
    }

    public class AssessmentResponseDto
    {
        public int Id { get; set; }
        public string ClientId { get; set; } = string.Empty;
        public string AssessorName { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Condition { get; set; } = string.Empty;
        public int ChickenCount { get; set; }
        public string? Notes { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime SyncedAt { get; set; }
        public List<PhotoResponseDto> Photos { get; set; } = new();
    }

    public class PhotoResponseDto
    {
        public int Id { get; set; }
        public string CloudinaryUrl { get; set; } = string.Empty;
        public string Filename { get; set; } = string.Empty;
        public DateTime CapturedAt { get; set; }
    }

    public class AssessmentStatsDto
    {
        public int Total { get; set; }
        public int Good { get; set; }
        public int Moderate { get; set; }
        public int Bad { get; set; }
        public int TotalChickens { get; set; }
        public int TotalFarms { get; set; }
        public int PendingFarms { get; set; }
        public int InProgressFarms { get; set; }
        public int CompletedFarms { get; set; }
    }
}