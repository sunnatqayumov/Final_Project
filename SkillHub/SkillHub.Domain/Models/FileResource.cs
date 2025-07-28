namespace SkillHub.Models;

public class FileResource
{
    public int Id { get; set; }
    public string FileName { get; set; } = string.Empty;
    public string FilePath { get; set; } = string.Empty;
    public int SessionId { get; set; }
    public MentorshipSession? Session { get; set; }
    public DateTime UploadedAt { get; set; } = DateTime.UtcNow;
}
