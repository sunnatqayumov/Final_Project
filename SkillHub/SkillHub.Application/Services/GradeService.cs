using Microsoft.EntityFrameworkCore;
using SkillHub.Data;
using SkillHub.DTOs;
using SkillHub.Interfaces;
using SkillHub.Models;

public class GradeService : IGradeService
{
    private readonly SkillHubDbContext _context;

    public GradeService(SkillHubDbContext context)
    {
        _context = context;
    }

    public async Task<GradeDto> CreateAsync(int mentorId, GradeCreateDto dto)
    {
        var grade = new Grade
        {
            Value = dto.Value,
            Comment = dto.Comment,
            LearnerId = dto.LearnerId,
            MentorId = mentorId,
            MentorshipSessionId = dto.MentorshipSessionId
        };

        _context.Grades.Add(grade);
        await _context.SaveChangesAsync();

        return MapToDto(grade);
    }

    public async Task<IEnumerable<GradeDto>> GetAllAsync()
    {
        return await _context.Grades
            .Select(g => MapToDto(g))
            .ToListAsync();
    }

    public async Task<IEnumerable<GradeDto>> GetByLearnerIdAsync(int learnerId)
    {
        return await _context.Grades
            .Where(g => g.LearnerId == learnerId)
            .Select(g => MapToDto(g))
            .ToListAsync();
    }

    public async Task<GradeDto> UpdateAsync(int gradeId, int mentorId, GradeUpdateDto dto)
    {
        var grade = await _context.Grades.FirstOrDefaultAsync(g => g.Id == gradeId);

        if (grade == null)
            throw new Exception("Grade not found.");

        if (grade.MentorId != mentorId)
            throw new Exception("You are not allowed to update this grade.");

        grade.Value = dto.Value;
        grade.Comment = dto.Comment;

        await _context.SaveChangesAsync();
        return MapToDto(grade);
    }

    public async Task<string> DeleteAsync(int gradeId, int mentorId)
    {
        var grade = await _context.Grades.FirstOrDefaultAsync(g => g.Id == gradeId);

        if (grade == null)
            return "Grade not found.";

        if (grade.MentorId != mentorId)
            return "You are not allowed to delete this grade.";

        _context.Grades.Remove(grade);
        await _context.SaveChangesAsync();
        return "Grade deleted successfully.";
    }

    public async Task<List<LearnerRatingDto>> GetTopLearnersAsync(int count)
    {
        return await _context.Grades
            .GroupBy(g => g.LearnerId)
            .Select(g => new LearnerRatingDto
            {
                LearnerId = g.Key,
                LearnerName = g.FirstOrDefault()!.Learner.Name,
                AverageGrade = g.Average(x => x.Value),
                TotalGrades = g.Count()
            })
            .OrderByDescending(l => l.AverageGrade)
            .Take(count)
            .ToListAsync();
    }
    private static GradeDto MapToDto(Grade g) => new()
    {
        Id = g.Id,
        Value = g.Value,
        Comment = g.Comment,
        MentorId = g.MentorId,
        LearnerId = g.LearnerId,
        MentorshipSessionId = g.MentorshipSessionId,
        CreatedAt = g.CreatedAt
    };
}
