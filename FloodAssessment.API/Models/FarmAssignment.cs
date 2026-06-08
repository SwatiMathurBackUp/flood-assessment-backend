namespace FloodAssessment.API.Models
{
    public class FarmAssignment
    {
        public int Id { get; set; }
        public string FarmName { get; set; } = string.Empty;
        public string OwnerName { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public int EstimatedChickens { get; set; }
        public string Status { get; set; } = FarmStatus.Pending; // Pending / InProgress / Completed
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? CompletedAt { get; set; }

        // Foreign Keys
        public int? AssignedToUserId { get; set; }
        public User? AssignedTo { get; set; } = null!;

        public int? AssessmentId { get; set; }
        public Assessment? Assessment { get; set; }
    }
}