using IAmAGame_Backend.Engine.Interface;

namespace IAmAGame_Backend.Engine.GifParty;

public class Player : IPlayer
{
    public string Name { get; set; }
    public int Score { get; set; }
}

