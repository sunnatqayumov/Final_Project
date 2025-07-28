using System.ComponentModel.DataAnnotations;

namespace SkillHub.Models;

public class Review
{
    public int Id { get; set; }
    public int Rating { get; set; }
    public required string Comment { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public int LearnerId { get; set; }
    public required User Learner { get; set; }
    public int SessionId { get; set; }
    public required MentorshipSession Session { get; set; }
    public bool IsFlagged { get; set; } = false;
}
