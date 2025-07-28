using SkillHub.DTOs;
using SkillHub.Models;

namespace SkillHub.Interfaces;

public interface ITagService
{
    Task<Tag> CreateAsync(TagCreateDto dto);
    Task<IEnumerable<Tag>> GetAllAsync();
}
