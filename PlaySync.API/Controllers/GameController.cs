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

    [HttpDelete("delete/{code}")]
    public async Task<IActionResult> DeleteRoom(string code)
    {
        int id = GetUserId();

        var result = await _service.DeleteRoom(code, id);
        return result switch
        {
            DeleteRoomResult.Success => Ok(),
            DeleteRoomResult.RoomNotFound => NotFound(),
            DeleteRoomResult.PlayersStillInRoom => BadRequest(),
            DeleteRoomResult.UserNotHost => Unauthorized(),
            _ => throw new ArgumentOutOfRangeException(nameof(result), result, null),
        };
    }

    [HttpPost("join/{code}")]
    public async Task<IActionResult> JoinRoom(string code)
    {
        int id = GetUserId();

        var result = await _service.JoinRoom(code, id);
        return result switch
        {
            JoinRoomResult.Success => Ok(),
            JoinRoomResult.RoomNotFound => NotFound(),
            JoinRoomResult.AlreadyJoined => BadRequest(),
            JoinRoomResult.RoomFull => BadRequest(),
            _ => throw new ArgumentOutOfRangeException(nameof(result), result, null),
        };
    }

    [HttpPost("leave/{code}")]
    public async Task<IActionResult> LeaveRoom(string code)
    {
        int id = GetUserId();

        var result = await _service.LeaveRoom(code, id);
        return result switch
        {
            LeaveRoomResult.Success => Ok(),
            LeaveRoomResult.RoomNotFound => NotFound(),
            LeaveRoomResult.PlayerNotInRoom => BadRequest(),
            _ => throw new ArgumentOutOfRangeException(nameof(result), result, null),
        };
    }

    [HttpGet]
    public async Task<IActionResult> GetRooms()
    {
        var result = await _service.GetRooms();
        return Ok(result);
    }
}
