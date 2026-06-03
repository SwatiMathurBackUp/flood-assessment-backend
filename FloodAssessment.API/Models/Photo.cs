namespace FloodAssessment.API.Models
{
    public class Photo
    {
        public int Id { get; set; }
        public string CloudinaryUrl { get; set; } = string.Empty;
        public string CloudinaryPublicId { get; set; } = string.Empty;
        public string Filename { get; set; } = string.Empty;
        public DateTime CapturedAt { get; set; } = DateTime.UtcNow;

        // Foreign key
        public int AssessmentId { get; set; }
        public Assessment Assessment { get; set; } = null!;
    }
}