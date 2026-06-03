namespace FloodAssessment.API.DTOs
{
    public class FarmAssignmentResponseDto
    {
        public int Id { get; set; }
        public string FarmName { get; set; } = string.Empty;
        public string OwnerName { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public int EstimatedChickens { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime? CompletedAt { get; set; }
        public int? AssessmentId { get; set; }
        public string AssignedToName { get; set; } = string.Empty;
    }

    public class UpdateFarmStatusDto
    {
        public string Status { get; set; } = string.Empty;
    }

    public class MapPinDto
    {
        public int Id { get; set; }
        public string FarmName { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Status { get; set; } = string.Empty;
        public string AssignedToName { get; set; } = string.Empty;
        public string? Condition { get; set; }
        public int? ChickenCount { get; set; }
    }
}