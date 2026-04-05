using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AuthService _auth;
    private readonly AppDbContext _context;

    public AuthController(AuthService auth, AppDbContext context)
    {
        _auth = auth;
        _context = context;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto dto)
    {
        await _auth.Register(dto.Username, dto.Password);
        return Ok();
    }

    [HttpPost("login")]
    public IActionResult Login(RegisterDto dto)
    {
        var user = _context.Users.FirstOrDefault(u => u.Username == dto.Username);

        if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
            return Unauthorized();

        var token = _auth.GenerateJwt(user);
        if (token == null)
        {
            return Problem();
        }
        return Ok(new { token });
    }
}
