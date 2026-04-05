using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

[ApiController]
[Route("api/[controller]")]
public class GameController : ControllerBase
{
    private readonly GameService _service;

    public GameController(GameService service)
    {
        _service = service;
    }

    private int? GetUserId()
    {
        var claim = User.FindFirst(ClaimTypes.NameIdentifier);

        if (claim == null || string.IsNullOrWhiteSpace(claim.Value))
            return null;

        if (int.TryParse(claim.Value, out var userId))
            return userId;

        return null;
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateRoom()
    {
        var id = GetUserId()
            ?? throw new UnauthorizedAccessException();

        var room = await _service.CreateRoom(id);
        return Ok(room);
    }

    [HttpPost("join/{code}")]
    public async Task<IActionResult> JoinRoom(string code)
    {
        var id = GetUserId()
            ?? throw new UnauthorizedAccessException();

        await _service.JoinRoom(code, id);
        return Ok();
    }

    [HttpGet]
    public IActionResult GetRooms()
    {
        int? id = GetUserId();
        if (id == null)
        {
            return Unauthorized();
        }

        return Ok(_service.GetRooms());
    }
}
