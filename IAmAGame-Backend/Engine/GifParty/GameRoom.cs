namespace IAmAGame_Backend.Engine.GifParty;

public class GameRoom
{
    public enum State { Lobby, Running, Ended };
    public State GameState { get; set; }
    public string Name { get; set; }
    public List<Player> Players { get; set; }
    public int MaxPlayers { get; set; }
    public DateTime StartedAt { get; set; }
    public List<Game> Games { get; set; }
    public Player? Host { get; set; }


    public GameRoom(string name)
    {
        Name = name;
        GameState = State.Lobby;
        Players = new List<Player>();
        MaxPlayers = 10;
        Games = new List<Game>(MaxPlayers);
        Host = null;
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

    public Player AddPlayer(string name)
    {
        var player = new Player(name);
        if (Players.Count >= MaxPlayers)
        {
            throw new Exception("Room is full");
        }
        else
        {
            Players.Add(player);

            Console.WriteLine($"Player {player.Name} added to room {Name}");
            Console.WriteLine($"Room {Name} now has {Players.Count} players");
        }

        return player;
    }

    public Player GetPlayer(Guid id)
    {
        var player = Players.Find(x => x.Id == id);
        _ = player ?? throw new NullReferenceException($"Player with id: {id}. Could not be found");

        return player;
    }

    public void RemovePlayer(Player player)
    {
        if (Players.Count == 0) { return; }
        Players.Remove(player);
    }


    public void SetHost(Player player)
    {
        Host = player;
    }

    public int GetTime()
    {
        DateTime currentTime = DateTime.Now;
        int timeInSeconds = (currentTime - StartedAt).Seconds;

        return timeInSeconds;
    }
}
