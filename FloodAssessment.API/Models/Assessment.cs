namespace FloodAssessment.API.Models
{
    public class Assessment
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
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime SyncedAt { get; set; } = DateTime.UtcNow;

        // Link to user
        public int? UserId { get; set; }
        public User? User { get; set; }

        // Navigation
        public List<Photo> Photos { get; set; } = new();
        public FarmAssignment? FarmAssignment { get; set; }
    }
}