namespace SkillHub.DTOs;

public class MentorshipSessionDto
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public required string Description { get; set; }
    public required string TimeFrame { get; set; }
    public int Capacity { get; set; }
    public int Difficulty { get; set; }
    public int MentorId { get; set; }
    public required string MentorName { get; set; }
    public List<string> Tags { get; set; } = new();
    public double AverageRating { get; set; }
}

