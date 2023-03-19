namespace IAmAGame_Backend.Engine.Interfaces;

public interface IGameRoom
{
    string Name { get; }
    List<IPlayer> Players { get; }
    int MaxPlayers { get; }
    DateTime StartedAt { get; }
    void Start();
    void End();
    void AddPlayer(IPlayer player);
    void RemovePlayer(IPlayer player);
    List<IPlayer> GetPlayers();
    int GetTime();
}

