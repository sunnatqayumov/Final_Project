using SkillHub.DTOs;

namespace SkillHub.Interfaces;

public interface IGradeService
{
    Task<GradeDto> CreateAsync(int mentorId, GradeCreateDto dto);
    Task<IEnumerable<GradeDto>> GetAllAsync();
    Task<IEnumerable<GradeDto>> GetByLearnerIdAsync(int learnerId);
    Task<GradeDto> UpdateAsync(int gradeId, int mentorId, GradeUpdateDto dto);
    Task<string> DeleteAsync(int gradeId, int mentorId);
    Task<List<LearnerRatingDto>> GetTopLearnersAsync(int count);
}
