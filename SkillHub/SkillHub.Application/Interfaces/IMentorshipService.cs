using SkillHub.DTOs;
using SkillHub.Models;

namespace SkillHub.Interfaces;

public interface IMentorshipSessionService
{
    Task<MentorshipSessionDto> CreateAsync(MentorshipSessionCreateDto dto);
    Task<List<MentorshipSessionDto>> GetAllAsync();
    Task<bool> DeleteAsync(int id);
    Task<bool> UpdateAsync(int id, MentorshipSessionCreateDto dto);
    Task<IEnumerable<MentorshipSession>> GetFilteredAsync(List<string>? tags = null);

}
