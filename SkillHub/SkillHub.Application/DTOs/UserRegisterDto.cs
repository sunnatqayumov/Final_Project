using SkillHub.Models;

namespace SkillHub.DTOs;

public class UserRegisterDto
{
    public required string Name { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }
    public Role Role { get; set; } = Role.Learner;
}
