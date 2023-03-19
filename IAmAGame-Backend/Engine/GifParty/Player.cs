using IAmAGame_Backend.Engine.Interfaces;

namespace IAmAGame_Backend.Engine.GifParty;

public class Player : IPlayer
{
    public Guid Id { get; } = new Guid();
    public string Name { get; set; } = String.Empty;
    public int Score { get; set; }

}

