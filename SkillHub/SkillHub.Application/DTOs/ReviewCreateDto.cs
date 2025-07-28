namespace SkillHub.DTOs;

public class ReviewCreateDto
{
    public int SessionId { get; set; }
    public int Rating { get; set; }
    public required string Comment { get; set; }
}
