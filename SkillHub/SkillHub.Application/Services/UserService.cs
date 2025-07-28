using SkillHub.DTOs;
using SkillHub.Models;
using SkillHub.Interfaces;
using SkillHub.Data;
using BCrypt.Net;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;

namespace SkillHub.Services;

public class UserService : IUserService
{

    private readonly SkillHubDbContext _context;
    private readonly IConfiguration _configuration;

    public UserService(SkillHubDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }
    public async Task<string> RegisterAsync(UserRegisterDto userRegisterDto)
    {
        if (await _context.Users.AnyAsync(u => u.Email == userRegisterDto.Email))
            return "User with this email already exists";

        var user = new User
        {
            Name = userRegisterDto.Name,
            Email = userRegisterDto.Email,
            Role = userRegisterDto.Role,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(userRegisterDto.Password)
        };
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return GenerateJwt(user);
    }

    public async Task<string> LoginAsync(LoginDto loginDto)
    {
        if (string.IsNullOrEmpty(loginDto.Email) || string.IsNullOrEmpty(loginDto.Password))
            return "Invalid email or password";

        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == loginDto.Email);
        if (user == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash))
            return "Invalid email or password";

        return GenerateJwt(user);
    }

    private string GenerateJwt(User user)
    {
        var claims = new[]
        {
        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        new Claim(ClaimTypes.Role, user.Role.ToString()),
        new Claim(ClaimTypes.Name, user.Name),
    };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.UtcNow.AddHours(4),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

}

