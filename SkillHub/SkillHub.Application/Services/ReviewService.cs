using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using SkillHub.Data;
using SkillHub.DTOs;
using SkillHub.Interfaces;
using SkillHub.Models;

namespace SkillHub.Services;

public class ReviewService : IReviewService
{
    private readonly SkillHubDbContext _context;

    public ReviewService(SkillHubDbContext context)
    {
        _context = context;
    }

    public async Task<ReviewDto> CreateAsync(int userId, ReviewCreateDto dto)
    {
        
        var exists = await _context.Reviews.AnyAsync(r => r.LearnerId == userId && r.SessionId == dto.SessionId);
        if (exists)
            throw new Exception("We've already received your review for this session.");

        var learner = await _context.Users.FindAsync(userId);
        var session = await _context.MentorshipSessions.FindAsync(dto.SessionId);

        if (learner == null)
            throw new Exception("Learner not found.");
        if (session == null)
            throw new Exception("Session not found.");

        var review = new Review
        {
            SessionId = dto.SessionId,
            LearnerId = userId,
            Rating = dto.Rating,
            Comment = dto.Comment,
            CreatedAt = DateTime.UtcNow,
            Learner = learner,
            Session = session
        };

        _context.Reviews.Add(review);
        await _context.SaveChangesAsync();

        return new ReviewDto
        {
            Id = review.Id,
            SessionId = review.SessionId,
            LearnerId = review.LearnerId,
            Rating = review.Rating,
            Comment = review.Comment,
            CreatedAt = review.CreatedAt,
            IsFlagged = review.IsFlagged
        };
    }

    public async Task<IEnumerable<ReviewDto>> GetBySessionIdAsync(int sessionId)
    {
        var reviews = await _context.Reviews
            .Where(r => r.SessionId == sessionId)
            .ToListAsync();

        return reviews.Select(r => new ReviewDto
        {
            Id = r.Id,
            SessionId = r.SessionId,
            LearnerId = r.LearnerId,
            Rating = r.Rating,
            Comment = r.Comment,
            CreatedAt = r.CreatedAt,
            IsFlagged = r.IsFlagged
        });
    }

    public async Task<string> DeleteAsync(int reviewId, int userId)
    {
        var review = await _context.Reviews.FirstOrDefaultAsync(r => r.Id == reviewId);

        if (review == null)
            return "Review not found.";

        if (review.LearnerId != userId)
            return "You are not allowed to delete this review.";

        _context.Reviews.Remove(review);
        await _context.SaveChangesAsync();
        return "Review deleted successfully.";
    }
}
