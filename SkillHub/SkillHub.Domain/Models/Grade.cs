namespace SkillHub.Models;
public class Grade
{
    public int Id { get; set; }

    public int Value { get; set; } // например, от 1 до 5

    public required string Comment { get; set; }

    public int MentorId { get; set; }
    public User Mentor { get; set; } = null!;

    public int LearnerId { get; set; }
    public User Learner { get; set; } = null!;

    public int MentorshipSessionId { get; set; }
    public MentorshipSession MentorshipSession { get; set; } = null!;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
