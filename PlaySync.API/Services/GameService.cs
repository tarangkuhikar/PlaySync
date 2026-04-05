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
            HostId = userId
        };

        _context.GameRooms.Add(room);
        await _context.SaveChangesAsync();

        var player = new GamePlayer
        {
            UserId = userId,
            GameRoomId = room.Id
        };

        _context.GamePlayers.Add(player);
        await _context.SaveChangesAsync();

        return room;
    }

    public async Task JoinRoom(string roomCode, int userId)
    {
        var room = _context.GameRooms.FirstOrDefault(r => r.RoomCode == roomCode);

        if (room == null)
            throw new Exception("Room not found");

        var player = new GamePlayer
        {
            UserId = userId,
            GameRoomId = room.Id
        };

        _context.GamePlayers.Add(player);
        await _context.SaveChangesAsync();
    }

    public List<GameRoom> GetRooms()
    {
        return _context.GameRooms.ToList();
    }
}
