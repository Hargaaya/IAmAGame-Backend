namespace IAmAGame_Backend.Engine.Interfaces;
using IAmAGame_Backend.Engine.GifParty;

public interface IGameRoom
{
  string Name { get; }
  List<Player> Players { get; }
  int MaxPlayers { get; }
  DateTime StartedAt { get; }
  void Start();
  void End();
  void AddPlayer(Player player);
  void RemovePlayer(Player player);
  List<Player> GetPlayers();
  int GetTime();
}

