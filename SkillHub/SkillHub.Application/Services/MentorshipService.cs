using SkillHub.DTOs;
using SkillHub.Models;
using SkillHub.Interfaces;
using Microsoft.EntityFrameworkCore;
using SkillHub.Data;

namespace SkillHub.Services;

public class MentorshipSessionService : IMentorshipSessionService
{
    private readonly SkillHubDbContext _context;

    public MentorshipSessionService(SkillHubDbContext context)
    {
        _context = context;
    }

    public async Task<MentorshipSessionDto> CreateAsync(MentorshipSessionCreateDto dto)
    {
        var session = new MentorshipSession
        {
            Title = dto.Title,
            Description = dto.Description,
            TimeFrame = dto.TimeFrame,
            Capacity = dto.Capacity,
            Difficulty = dto.Difficulty,
            MentorId = dto.MentorId,
            SessionTags = dto.TagIds?.Select(id => new SessionTag { TagId = id }).ToList() ?? new()
        };

        _context.MentorshipSessions.Add(session);
        await _context.SaveChangesAsync();

        var tagNames = await _context.Tags
            .Where(t => dto.TagIds!.Contains(t.Id))
            .Select(t => t.Name)
            .ToListAsync();

        return new MentorshipSessionDto
        {
            Id = session.Id,
            Title = session.Title,
            MentorName = session.Mentor!.Name,
            Description = session.Description,
            TimeFrame = session.TimeFrame,
            Capacity = session.Capacity,
            Difficulty = session.Difficulty,
            MentorId = session.MentorId,
            Tags = tagNames
        };
    }

    public async Task<List<MentorshipSessionDto>> GetAllAsync()
    {
        return await _context.MentorshipSessions
            .Include(ms => ms.SessionTags)
                .ThenInclude(st => st.Tag)
            .Select(ms => new MentorshipSessionDto
            {
                Id = ms.Id,
                Title = ms.Title,
                MentorName = ms.Mentor!.Name,
                Description = ms.Description,
                TimeFrame = ms.TimeFrame,
                Capacity = ms.Capacity,
                Difficulty = ms.Difficulty,
                MentorId = ms.MentorId,
                Tags = ms.SessionTags.Select(st => st.Tag!.Name).ToList()
            })
            .ToListAsync();
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var session = await _context.MentorshipSessions.FindAsync(id);
        if (session == null) return false;

        _context.MentorshipSessions.Remove(session);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> UpdateAsync(int id, MentorshipSessionCreateDto dto)
    {
        var session = await _context.MentorshipSessions
            .Include(s => s.SessionTags)
            .FirstOrDefaultAsync(s => s.Id == id);

        if (session == null) return false;

        session.Title = dto.Title;
        session.Description = dto.Description;
        session.Difficulty = dto.Difficulty;
        session.TimeFrame = dto.TimeFrame;
        session.Capacity = dto.Capacity;
        session.MentorId = dto.MentorId;

        _context.SessionTags.RemoveRange(session.SessionTags);

        foreach (var tagId in dto.TagIds!)
        {
            session.SessionTags.Add(new SessionTag
            {
                SessionId = session.Id,
                TagId = tagId
            });
        }

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<MentorshipSession>> GetFilteredAsync(List<string>? tags = null)
    {
        var query = _context.MentorshipSessions
            .Include(s => s.Mentor)
            .Include(s => s.SessionTags)
            .ThenInclude(st => st.Tag)
            .AsQueryable();

        if (tags != null && tags.Any())
        {
            query = query.Where(s => s.SessionTags.Any(t => tags.Contains(t.Tag!.Name)));
        }

        return await query.ToListAsync();
    }
}
