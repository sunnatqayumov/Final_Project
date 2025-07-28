namespace SkillHub.Models;

public class Enrollment
{
    public int Id { get; set; }
    public int LearnerId { get; set; }
    public User? Learner { get; set; }
    public int SessionId { get; set; }
    public MentorshipSession? Session { get; set; }
    public DateTime EnrolledAt { get; set; } = DateTime.UtcNow;
    public bool IsCompleted { get; set; } = false;
}
