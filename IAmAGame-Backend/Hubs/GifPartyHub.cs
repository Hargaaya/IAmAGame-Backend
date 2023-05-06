using IAmAGame_Backend.Engine.GifParty;
using IAmAGame_Backend.Persistance;
using Microsoft.AspNetCore.SignalR;

namespace IAmAGame_Backend.Hubs;

public class GifPartyHub : Hub
{

  private RedisDatabase _db;

  public GifPartyHub()
  {
    _db = new RedisDatabase();
  }

  public void GetGame(string key)
  {
    var game = _db.Get<GameRoom>(key);

    if (game == null)
    {
      Clients.Client(Context.ConnectionId).SendAsync("ReceiveGame", "error");
      Console.WriteLine("game is null, sending error");
      return;
    }

    Clients.Client(Context.ConnectionId).SendAsync("ReceiveGame", game);
  }

  public void AddPlayer(string key, string name)
  {
    var game = _db.Get<GameRoom>(key);

    if (game == null) return;

    var player = new Player
    {
      Name = name,
      Score = 0,
    };

    game.AddPlayer(player);

    _db.Set(key, game);

    //send back game data to the player who joined
    Clients.Client(Context.ConnectionId).SendAsync("ReceivePlayer", player);
    Clients.Client(Context.ConnectionId).SendAsync("ReceiveGame", game);

    //send new player to all other players
    Clients.Group(key).SendAsync("PlayerJoined", player);

    //add player to group
    Groups.AddToGroupAsync(Context.ConnectionId, key);
  }

  public void RemovePlayer(string key, Player player)
  {
    var game = _db.Get<GameRoom>(key);

    if (game == null) return;

    game.RemovePlayer(player);

    _db.Set(key, game);

    //send that player left to all other players
    Clients.Group(key).SendAsync("PlayerLeft", player);

    //remove player from group
    Groups.RemoveFromGroupAsync(Context.ConnectionId, key);
  }

  public void GetPlayer(string key, Guid id)
  {
    var game = _db.Get<GameRoom>(key);

    if (game == null) return;

    var players = game.GetPlayers();

    var player = players.Find(player => player.Id == id);

    if (player == null) return;

    Clients.Client(Context.ConnectionId).SendAsync("ReceivePlayer", player);
  }

  public void GetPlayers(string key)
  {
    var game = _db.Get<GameRoom>(key);

    if (game == null) return;

    var players = game.GetPlayers();

    Console.WriteLine("players in game " + game.Name);
    Console.WriteLine("players count " + players.Count);

    foreach (var player in players)
    {
      Console.WriteLine("player " + player.Name);
    }
  }

  public void SetPlayerReadyState(string key, Player player, bool ready)
  {
    var game = _db.Get<GameRoom>(key);

    if (game == null) return;

    game.SetPlayerReadyState(player, ready);

    _db.Set(key, game);

    var players = game.GetPlayers();

    Clients.Group(key).SendAsync("ReceiveGame", game);
  }

  public void AddScreen(string key)
  {
    var game = _db.Get<GameRoom>(key);

    if (game == null) return;

    Groups.AddToGroupAsync(Context.ConnectionId, key);
  }
}
