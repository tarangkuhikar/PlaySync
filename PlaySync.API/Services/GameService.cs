using Microsoft.EntityFrameworkCore;

public class GameService
{
    private readonly AppDbContext _context;

    public GameService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<GameRoom> CreateRoom(int userId)
    {
        var room = new GameRoom
        {
            RoomCode = Guid.NewGuid().ToString().Substring(0, 6),
            HostId = userId,
        };

        await _context.GameRooms.AddAsync(room);
        await _context.SaveChangesAsync();

        var player = new GamePlayer { UserId = userId, GameRoomId = room.Id };

        await _context.GamePlayers.AddAsync(player);
        await _context.SaveChangesAsync();

        return room;
    }

    public async Task<bool> JoinRoom(string roomCode, int userId)
    {
        var room = await _context.GameRooms.FirstOrDefaultAsync(r => r.RoomCode == roomCode);

        if (room == null)
            return false;

        var player = new GamePlayer { UserId = userId, GameRoomId = room.Id };

        await _context.GamePlayers.AddAsync(player);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<List<GameRoom>> GetRooms()
    {
        return await _context.GameRooms.ToListAsync();
    }
}
