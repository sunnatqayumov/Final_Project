using Microsoft.EntityFrameworkCore;
using SkillHub.Models;

namespace SkillHub.Infrastructure.Data;

public class SkillHubDbContext(DbContextOptions<SkillHubDbContext> options) : DbContext(options), DbContext
{
    public DbSet<User> Users => Set<User>();
    public DbSet<MentorshipSession> MentorshipSessions => Set<MentorshipSession>();
    public DbSet<Enrollment> Enrollments => Set<Enrollment>();
    public DbSet<Review> Reviews => Set<Review>();
    public DbSet<FileResource> FileResources => Set<FileResource>();
    public DbSet<Tag> Tags => Set<Tag>();
    public DbSet<SessionTag> SessionTags => Set<SessionTag>();
    public DbSet<Grade> Grades => Set<Grade>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder
            .Entity<User>()
            .Property(u => u.Role)
            .HasConversion<string>();

        modelBuilder
            .Entity<MentorshipSession>()
            .Property(s => s.Difficulty)
            .HasConversion<string>();


        modelBuilder.Entity<SessionTag>()
            .HasKey(st => new { st.SessionId, st.TagId });

        modelBuilder.Entity<SessionTag>()
            .HasOne(st => st.Session)
            .WithMany(s => s.SessionTags)
            .HasForeignKey(st => st.SessionId);

        modelBuilder.Entity<SessionTag>()
            .HasOne(st => st.Tag)
            .WithMany(t => t.SessionTags)
            .HasForeignKey(st => st.TagId);


        modelBuilder.Entity<Enrollment>()
            .HasIndex(e => new { e.LearnerId, e.SessionId })
            .IsUnique();


        modelBuilder.Entity<Review>()
            .HasIndex(r => new { r.LearnerId, r.SessionId })
            .IsUnique();

        modelBuilder.Entity<Tag>()
            .HasIndex(t => t.Name)
            .IsUnique();
    }
}