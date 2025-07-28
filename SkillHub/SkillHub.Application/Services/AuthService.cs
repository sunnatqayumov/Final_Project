using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SkillHub.Data;
using SkillHub.DTOs;
using SkillHub.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace SkillHub.Services;

public class AuthService : IAuthService
{
    private readonly SkillHubDbContext _context;
    private readonly IConfiguration _config;

    public AuthService(SkillHubDbContext context, IConfiguration config)
    {
        _context = context;
        _config = config;
    }

    public async Task<string> RegisterAsync(RegisterDto dto)
    {
        if (await _context.Users.AnyAsync(u => u.Email == dto.Email))
            return "Email already registered";

        if(await _context.Users.AnyAsync(u => u.Name == dto.Name))
            return "Username already taken";

        var user = new Models.User
        {
            Name = dto.Name,
            Email = dto.Email,
            Role = Enum.Parse<Models.Role>(dto.Role, true),
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password)
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return GenerateJwt(user);
    }

    public async Task<string> LoginAsync(LoginDto dto)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);

        if (!BCrypt.Net.BCrypt.Verify(dto.Password, user!.PasswordHash))
            return "Invalid credentials";

        return GenerateJwt(user);
    }

    public async Task<string> GetAllAsync()
    {
        var users = await _context.Users
            .Select(u => new { u.Id, u.Name, u.Email, u.Role })
            .ToListAsync();

        return JsonSerializer.Serialize(users);
    }

    public async Task<string> DeleteAsync(int userId)
    {
        var user = await _context.Users.FindAsync(userId);
        if (user == null)
            return "User not found";

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();

        return "User deleted successfully";
    }

    private string GenerateJwt(Models.User user)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Role, user.Role.ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.UtcNow.AddDays(7),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
