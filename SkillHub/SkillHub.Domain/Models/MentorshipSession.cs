namespace SkillHub.Models;

public class MentorshipSession
{
    public int Id { get; set; }

    public required string Title { get; set; }
    public required string Description { get; set; }
    public required int Difficulty { get; set; }
    public required string TimeFrame { get; set; }
    public required int Capacity { get; set; }

    public required int MentorId { get; set; }
    public User? Mentor { get; set; }
    public List<SessionTag> SessionTags { get; set; } = new();



}
