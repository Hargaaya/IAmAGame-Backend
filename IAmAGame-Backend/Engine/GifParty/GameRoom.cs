using IAmAGame_Backend.Engine.Interface;

namespace IAmAGame_Backend.Engine.GifParty;

public class GameRoom : IGameRoom
{
    public enum State { Lobby, Running, Ended };
    public State GameState { get; set; }
    public string Name { get; }
    public List<IPlayer> Players { get; set; } = new List<IPlayer>();
    public int MaxPlayers { get; } = 10;
    public DateTime StartedAt { get; set; }
    public List<Game> Games { get; set; } = new List<Game>();

    public GameRoom(string name)
    {
        Name = name;
        GameState = State.Lobby;
    }

    public void Start()
    {
        StartedAt = DateTime.Now;
        GameState = State.Running;
    }

    public void End()
    {
        GameState = State.Ended;
    }

    public void AddPlayer(IPlayer player)
    {
        Players.Add(player);
    }

    public void RemovePlayer(IPlayer player)
    {
        Players.Remove(player);
    }

    public List<IPlayer> GetPlayers()
    {
        return Players;
    }

    public int GetTime()
    {
        DateTime currentTime = DateTime.Now;
        int timeInSeconds = (currentTime - StartedAt).Seconds;
        return timeInSeconds;
    }
}

