using IAmAGame_Backend.Engine.Interfaces;

namespace IAmAGame_Backend.Engine.GifParty;

public class GameRoom : IGameRoom
{
    public enum State { Lobby, Running, Ended };
    public State GameState { get; set; }
    public string Name { get; }
    public List<IPlayer> Players { get; set; }
    public int MaxPlayers { get; set; }
    public DateTime StartedAt { get; set; }
    public List<Game> Games { get; set; }

    public GameRoom(string name)
    {
        Name = name;
        GameState = State.Lobby;
        Players = new List<IPlayer>();
        MaxPlayers = 10;
        Games = new List<Game>(MaxPlayers);
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
        if (Players.Count >= MaxPlayers)
        {
            throw new Exception("Room is full");
        }
        else
        {

            Players.Add(player);
        }
    }

    public void RemovePlayer(IPlayer player)
    {
        if (Players.Count == 0) { return; }
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
