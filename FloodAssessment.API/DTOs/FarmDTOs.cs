using System.ComponentModel.DataAnnotations;
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
        public int? AssignedToUserId { get; set; }  // ✅ ADD THIS
        public string AssignedToUserName { get; set; } = string.Empty;
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
    public class CreateFarmDto
{
    [Required]
    [MaxLength(200)]
    public string FarmName { get; set; } = string.Empty;

    [Required]
    [MaxLength(200)]
    public string OwnerName { get; set; } = string.Empty;

    [Required]
    [MaxLength(500)]
    public string Address { get; set; } = string.Empty;

    public double Latitude { get; set; }

    public double Longitude { get; set; }

    [Range(1, int.MaxValue)]
    public int EstimatedChickens { get; set; }
}
public class UpdateFarmDto
{
    [Required]
    [MaxLength(200)]
    public string FarmName { get; set; } = string.Empty;

    [Required]
    [MaxLength(200)]
    public string OwnerName { get; set; } = string.Empty;

    [Required]
    [MaxLength(500)]
    public string Address { get; set; } = string.Empty;

    public double Latitude { get; set; }

    public double Longitude { get; set; }

    [Range(1, int.MaxValue)]
    public int EstimatedChickens { get; set; }
}

public class AssignFarmDto
{
    [Required]
    public int AssessorUserId { get; set; }
}

public class FarmDto
{
    public int Id { get; set; }

    public string FarmName { get; set; } = string.Empty;

    public string OwnerName { get; set; } = string.Empty;

    public string Address { get; set; } = string.Empty;

    public int EstimatedChickens { get; set; }

    public string Status { get; set; } = string.Empty;

    public int? AssignedToUserId { get; set; }

    public string? AssignedToUserName { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? CompletedAt { get; set; }
}public class FarmDetailsDto
{
    public int Id { get; set; }

    public string FarmName { get; set; } = string.Empty;

    public string OwnerName { get; set; } = string.Empty;

    public string Address { get; set; } = string.Empty;

    public double Latitude { get; set; }

    public double Longitude { get; set; }

    public int EstimatedChickens { get; set; }

    public string Status { get; set; } = string.Empty;

    public int? AssignedToUserId { get; set; }

    public string? AssignedToUserName { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? CompletedAt { get; set; }
}
public class AssessorDto
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;
}
}