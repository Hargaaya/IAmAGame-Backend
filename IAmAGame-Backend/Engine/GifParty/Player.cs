using IAmAGame_Backend.Engine.Interfaces;

namespace IAmAGame_Backend.Engine.GifParty;

public class Player : IPlayer
{
  private Guid _id { get; set; } = Guid.Empty;
  public Guid Id
  {
    get
    {
      if (_id == Guid.Empty)
      {
        _id = Guid.NewGuid();
      }
      return _id;
    }
    set
    {
      _id = value;
    }
  }
  public string Name { get; set; } = String.Empty;
  public bool IsReady { get; set; } = false;
  public int Score { get; set; }

}

