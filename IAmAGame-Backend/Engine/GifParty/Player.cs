namespace IAmAGame_Backend.Engine.GifParty;

public class Player
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
    set => _id = value;
  }
  public string Name { get; set; } = String.Empty;
  public bool IsReady { get; set; } = false;
  public bool HasVoted { get; set; } = false;
  public int Score { get; set; } = 0;
  public string ConnectionId { get; set; } = String.Empty;

  public Player(string name)
  {
    Name = name;
  }

  public void AddScore()
  {
    Score++;
  }

  public void RemoveScore()
  {
    if (Score <= 0)
    {
      Score--;
    }
  }
}

