using Microsoft.AspNetCore.Authorization;
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

    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto dto)
    {
        var success = await _auth.Register(dto.Username, dto.Password);

        if (!success)
            return BadRequest("Invalid username or password.");

        return Ok();
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login(RegisterDto dto)
    {
        var user = await _auth.ValidateUser(dto.Username, dto.Password);

        if (user == null)
            return Unauthorized();

        var token = _auth.GenerateJwt(user);

        return Ok(new { token });
    }
}
