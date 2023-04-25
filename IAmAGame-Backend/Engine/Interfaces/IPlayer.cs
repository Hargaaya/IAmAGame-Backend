namespace IAmAGame_Backend.Engine.Interfaces;

public interface IPlayer
{
  Guid Id { get; }
  string Name { get; }
  int Score { get; set; }
  bool IsReady { get; set; }
}

