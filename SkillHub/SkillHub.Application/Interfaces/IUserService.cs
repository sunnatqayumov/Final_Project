using SkillHub.DTOs;

namespace SkillHub.Interfaces;

public interface IUserService
{
    Task<string> RegisterAsync(UserRegisterDto userRegisterDto);
    Task<string> LoginAsync(LoginDto loginDto);
}