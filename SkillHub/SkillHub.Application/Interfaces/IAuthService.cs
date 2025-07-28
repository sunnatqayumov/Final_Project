using SkillHub.DTOs;

namespace SkillHub.Interfaces;

public interface IAuthService
{
    Task<string> RegisterAsync(RegisterDto dto);
    Task<string> LoginAsync(LoginDto dto);
    Task<string> GetAllAsync();
    Task<string> DeleteAsync(int id);
}
