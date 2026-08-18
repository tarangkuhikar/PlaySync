using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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

    private int GetUserId()
    {
        var claim = User.FindFirst(ClaimTypes.NameIdentifier);

        if (claim == null || !int.TryParse(claim.Value, out var userId))
        {
            throw new UnauthorizedAccessException("User ID claim is missing or invalid.");
        }

        return userId;
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateRoom()
    {
        int id = GetUserId();

        var room = await _service.CreateRoom(id);
        return Ok(room);
    }

    [HttpPost("join/{code}")]
    public async Task<IActionResult> JoinRoom(string code)
    {
        int id = GetUserId();

        var response = await _service.JoinRoom(code, id);
        return response ? Ok() : NotFound();
    }

    [HttpGet]
    public async Task<IActionResult> GetRooms()
    {
        var result = await _service.GetRooms();
        return Ok(result);
    }
}
