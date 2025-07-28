namespace SkillHub.DTOs;

public class LearnerRatingDto
{
    public int LearnerId { get; set; }
    public string? LearnerName { get; set; }
    public double AverageGrade { get; set; }
    public int TotalGrades { get; set; }
}
