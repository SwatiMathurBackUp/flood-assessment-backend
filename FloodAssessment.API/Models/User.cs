namespace FloodAssessment.API.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string PinHash { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty; // Manager / Assessor
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation
        public List<FarmAssignment> AssignedFarms { get; set; } = new();
    }
}