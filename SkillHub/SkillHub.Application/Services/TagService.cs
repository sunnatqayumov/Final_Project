using Microsoft.EntityFrameworkCore;
using SkillHub.Data;
using SkillHub.DTOs;
using SkillHub.Interfaces;
using SkillHub.Models;

namespace SkillHub.Services;

public class TagService : ITagService
{
    private readonly SkillHubDbContext _context;

    public TagService(SkillHubDbContext context)
    {
        _context = context;
    }

    public async Task<Tag> CreateAsync(TagCreateDto dto)
    {
        var tag = new Tag { Name = dto.Name };
        _context.Tags.Add(tag);
        await _context.SaveChangesAsync();
        return tag;
    }

    public async Task<IEnumerable<Tag>> GetAllAsync()
    {
        return await _context.Tags.ToListAsync();
    }
}
