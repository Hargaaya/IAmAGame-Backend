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
            return;
        }

        Console.WriteLine(game.Name);
    }

}

