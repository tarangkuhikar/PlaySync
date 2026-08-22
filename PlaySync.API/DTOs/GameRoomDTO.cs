public record GameCreateDto(string RoomCode, int HostId);

public record GameRoomDto(int Id, string RoomCode, int HostId, List<GamePlayerDto> Players);

public record GamePlayerDto(int Id, int UserId, string Username);
