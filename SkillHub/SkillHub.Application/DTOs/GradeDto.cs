namespace SkillHub.DTOs;

public class GradeDto
{
    public int Id { get; set; }
    public int Value { get; set; }
    public string Comment { get; set; } = string.Empty;
    public int MentorId { get; set; }
    public int LearnerId { get; set; }
    public int MentorshipSessionId { get; set; }
    public DateTime CreatedAt { get; set; }
}
