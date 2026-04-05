public class GamePlayer
{
    public int Id { get; set; }

    public int UserId { get; set; }
    public User User { get; set; }

    public int GameRoomId { get; set; }
}
