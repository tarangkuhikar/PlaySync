using Microsoft.EntityFrameworkCore;

public class GameService
{
    private readonly AppDbContext _context;

    public GameService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<GameCreateDto> CreateRoom(int userId)
    {
        var room = new GameRoom
        {
            RoomCode = Guid.NewGuid().ToString().Substring(0, 6),
            HostId = userId,
        };

        var player = new GamePlayer { UserId = userId, GameRoom = room };

        await _context.GameRooms.AddAsync(room);
        await _context.GamePlayers.AddAsync(player);

        await _context.SaveChangesAsync();

        return new GameCreateDto(room.RoomCode, room.HostId);
    }

    public async Task<DeleteRoomResult> DeleteRoom(string roomCode, int userId)
    {
        var room = await _context.GameRooms.FirstOrDefaultAsync(r => r.RoomCode == roomCode);

        if (room == null)
            return DeleteRoomResult.RoomNotFound;

        if (room.HostId != userId)
            return DeleteRoomResult.UserNotHost;

        var hasPlayers = await _context.GamePlayers.AnyAsync(p => p.GameRoomId == room.Id);

        if (hasPlayers)
            return DeleteRoomResult.PlayersStillInRoom;

        _context.GameRooms.Remove(room);
        await _context.SaveChangesAsync();

        return DeleteRoomResult.Success;
    }

    public async Task<JoinRoomResult> JoinRoom(string roomCode, int userId)
    {
        var room = await _context.GameRooms.FirstOrDefaultAsync(r => r.RoomCode == roomCode);

        if (room == null)
            return JoinRoomResult.RoomNotFound;

        var player = new GamePlayer { UserId = userId, GameRoom = room };

        await _context.GamePlayers.AddAsync(player);
        await _context.SaveChangesAsync();

        return JoinRoomResult.Success;
    }

    public async Task<LeaveRoomResult> LeaveRoom(string roomCode, int userId)
    {
        var room = await _context
            .GameRooms.Include(r => r.Players)
            .FirstOrDefaultAsync(r => r.RoomCode == roomCode);

        if (room == null)
            return LeaveRoomResult.RoomNotFound;

        var player = room.Players.Find(x => x.UserId == userId);

        if (player == null)
            return LeaveRoomResult.PlayerNotInRoom;

        room.Players.Remove(player);
        await _context.SaveChangesAsync();

        return LeaveRoomResult.Success;
    }

    public async Task<List<GameRoomDto>> GetRooms()
    {
        return await _context
            .GameRooms.Select(room => new GameRoomDto(
                room.Id,
                room.RoomCode,
                room.HostId,
                room.Players.Select(player => new GamePlayerDto(
                        player.Id,
                        player.UserId,
                        player.User.Username
                    ))
                    .ToList()
            ))
            .ToListAsync();
    }
}
