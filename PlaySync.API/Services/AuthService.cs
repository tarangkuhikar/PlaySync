using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

public class AuthService
{
    private readonly AppDbContext _context;
    private readonly SigningCredentials _signingCredentials;

    public AuthService(AppDbContext context, SigningCredentials signingCredentials)
    {
        _context = context;
        _signingCredentials = signingCredentials;
    }

    public async Task<bool> Register(string username, string password)
    {
        if (await _context.Users.AnyAsync(u => u.Username == username))
            return false;

        var user = new User
        {
            Username = username,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
        };

        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<User?> ValidateUser(string username, string password)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);

        if (user == null)
            return null;

        var valid = BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);

        return valid ? user : null;
    }

    public string GenerateJwt(User user)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Username),
        };

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.UtcNow.AddHours(2),
            signingCredentials: _signingCredentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
