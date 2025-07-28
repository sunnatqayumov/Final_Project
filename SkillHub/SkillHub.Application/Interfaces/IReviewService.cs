using SkillHub.DTOs;

namespace SkillHub.Interfaces;

public interface IReviewService
{
    Task<ReviewDto> CreateAsync(int userId, ReviewCreateDto dto);
    Task<IEnumerable<ReviewDto>> GetBySessionIdAsync(int sessionId);
    Task<string> DeleteAsync(int reviewId, int userId);
}
