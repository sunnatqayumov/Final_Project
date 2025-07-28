namespace SkillHub.DTOs;

public class GradeCreateDto
{
    public int Value { get; set; }
    public required string Comment { get; set; }

    public int LearnerId { get; set; }
    public int MentorshipSessionId { get; set; }
}
