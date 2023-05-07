using IAmAGame_Backend.Engine.GifParty;
using IAmAGame_Backend.Persistance;
using Microsoft.AspNetCore.SignalR;

namespace IAmAGame_Backend.Hubs;

public class GifPartyHub : Hub
{
    private IRedisDatabase _db;

    public GifPartyHub(IRedisDatabase db)
    {
        _db = db;
    }

    public void GetGame(string key)
    {
        try
        {
            var game = _db.Get<GameRoom>(key);

            Clients.Client(Context.ConnectionId).SendAsync("ReceiveGame", game);
        }
        catch (NullReferenceException ex)
        {
            Clients.Client(Context.ConnectionId).SendAsync("ReceiveGame", "error");
            Console.WriteLine($"GifPartyHub.GetGame: {ex.Message}");
        }
    }

    public void AddPlayer(string key, string name)
    {
        try
        {
            var game = _db.Get<GameRoom>(key);

            var player = game.AddPlayer(name);

            _db.Set(key, game);

            Console.WriteLine($"Player {player.Name}");

            //send back game data to the player who joined
            Clients.Client(Context.ConnectionId).SendAsync("ReceivePlayer", player);
            Clients.Client(Context.ConnectionId).SendAsync("ReceiveGame", game);

            //send new player to all other players
            Clients.Group(key).SendAsync("PlayerJoined", player);

            //add player to group
            Groups.AddToGroupAsync(Context.ConnectionId, key);
        }
        catch (NullReferenceException ex)
        {
            Console.WriteLine($"GifPartyHub.AddPlayer: {ex.Message}");
        }
    }

    public void RemovePlayer(string key, Player player)
    {
        try
        {
            var game = _db.Get<GameRoom>(key);

            game.RemovePlayer(player);

            _db.Set(key, game);

            //send that player left to all other players
            Clients.Group(key).SendAsync("PlayerLeft", player);

            //remove player from group
            Groups.RemoveFromGroupAsync(Context.ConnectionId, key);
        }
        catch (NullReferenceException ex)
        {
            Console.WriteLine($"GifPartyHub.RemovePlayer: {ex.Message}");
        }

    }

    public void GetPlayer(string key, Guid id)
    {
        try
        {
            var game = _db.Get<GameRoom>(key);

            var player = game.GetPlayer(id);

            Clients.Client(Context.ConnectionId).SendAsync("ReceivePlayer", player);
        }
        catch (NullReferenceException ex)
        {
            Console.WriteLine($"GifPartyHub.GetPlayer: {ex.Message}");
        }

    }

    public void GetPlayers(string key)
    {
        try
        {
            var game = _db.Get<GameRoom>(key);

            var players = game.Players;

            Console.WriteLine("players in game " + game.Name);
            Console.WriteLine("players count " + players.Count);
            players.ForEach(p => Console.WriteLine("player: " + p.Name));

            /*foreach (var player in players)
            {
                Console.WriteLine("player " + player.Name);
            }*/
        }
        catch (NullReferenceException ex)
        {
            Console.WriteLine($"GifPartyHub.GetPlayers: {ex.Message}");
        }

    }

    public void SetPlayerReadyState(string key, Player player, bool isReady)
    {
        try
        {
            var game = _db.Get<GameRoom>(key);

            game.GetPlayer(player.Id).IsReady = isReady;

            _db.Set(key, game);

            Clients.Group(key).SendAsync("ReceiveGame", game);
        }
        catch (NullReferenceException ex)
        {
            Console.WriteLine($"GifPartyHub.AddScreen: {ex.Message}");
        }
    }

    public void AddScreen(string key)
    {
        try
        {
            var game = _db.Get<GameRoom>(key);

            Groups.AddToGroupAsync(Context.ConnectionId, key);
        }
        catch (NullReferenceException ex)
        {
            Console.WriteLine($"GifPartyHub.AddScreen: {ex.Message}");
        }
    }
}
