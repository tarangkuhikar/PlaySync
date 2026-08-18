using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

[ApiController]
[Authorize]
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
        int? id = GetUserId();
        if (id is not int userId)
        {
            return Unauthorized();
        }

        var room = await _service.CreateRoom(userId);
        return Ok(room);
    }

    [HttpPost("join/{code}")]
    public async Task<IActionResult> JoinRoom(string code)
    {
        int? id = GetUserId();

        if (id is not int userId)
        {
            return Unauthorized();
        }

        var response = await _service.JoinRoom(code, userId);
        return response ? Ok() : NotFound();
    }

    [HttpGet]
    public async Task<IActionResult> GetRooms()
    {
        int? id = GetUserId();

        if (id is not int userId)
        {
            return Unauthorized();
        }

        var result = await _service.GetRooms();
        return Ok(result);
    }
}
