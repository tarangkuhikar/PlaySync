public class GameRoom
{
    public int Id { get; set; }
    public string RoomCode { get; set; }
    public int HostId { get; set; }
    public List<GamePlayer> Players { get; set; } = new();
}
