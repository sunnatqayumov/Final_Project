namespace SkillHub.DTOs;

public class ReviewDto
{
    public int Id { get; set; }
    public int SessionId { get; set; }
    public int LearnerId { get; set; }
    public int Rating { get; set; }
    public string? Comment { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsFlagged { get; set; }
}
