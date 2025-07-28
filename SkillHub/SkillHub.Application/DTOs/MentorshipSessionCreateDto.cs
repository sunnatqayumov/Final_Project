namespace SkillHub.DTOs;

public class MentorshipSessionCreateDto
{
    public required string Title { get; set; }
    public required string Description { get; set; }
    public required string TimeFrame { get; set; }
    public required int Capacity { get; set; }
    public required int Difficulty { get; set; }
    public required int MentorId { get; set; }
    public List<int>? TagIds { get; set; }
}
