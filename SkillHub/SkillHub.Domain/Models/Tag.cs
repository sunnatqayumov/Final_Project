namespace SkillHub.Models;

public class Tag
{
    public int Id { get; set; }
    public required string Name { get; set; }

    public List<SessionTag> SessionTags { get; set; } = new();
}
