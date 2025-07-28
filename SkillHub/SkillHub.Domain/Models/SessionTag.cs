namespace SkillHub.Models;
public class SessionTag
{
    public int SessionId { get; set; }
    public MentorshipSession? Session { get; set; }

    public int TagId { get; set; }
    public Tag? Tag { get; set; }
}