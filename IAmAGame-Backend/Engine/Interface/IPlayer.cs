namespace IAmAGame_Backend.Engine.Interface;

public interface IPlayer
{
    Guid Id { get; }
    string Name { get; }
    int Score { get; }
}

