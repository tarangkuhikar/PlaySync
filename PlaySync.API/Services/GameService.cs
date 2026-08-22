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

    public async Task<bool> DeleteRoom(string roomCode, int userId)
    {
        var room = await _context.GameRooms.FirstOrDefaultAsync(r => r.RoomCode == roomCode);

        if (room == null || room.HostId != userId)
            return false;

        var hasPlayers = await _context.GamePlayers.AnyAsync(p => p.GameRoomId == room.Id);

        if (hasPlayers)
            return false;

        _context.GameRooms.Remove(room);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> JoinRoom(string roomCode, int userId)
    {
        var room = await _context.GameRooms.FirstOrDefaultAsync(r => r.RoomCode == roomCode);

        if (room == null)
            return false;

        var player = new GamePlayer { UserId = userId, GameRoom = room };

        await _context.GamePlayers.AddAsync(player);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> LeaveRoom(string roomCode, int userId)
    {
        var room = await _context
            .GameRooms.Include(r => r.Players)
            .FirstOrDefaultAsync(r => r.RoomCode == roomCode);

        if (room == null || room.HostId == userId)
            return false;

        var player = room.Players.Find(x => x.UserId == userId);

        if (player == null)
            return false;

        room.Players.Remove(player);
        await _context.SaveChangesAsync();

        return true;
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
