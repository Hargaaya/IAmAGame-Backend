namespace IAmAGame_Backend.Engine.GifParty;

public class Game
{
  public enum State { ChoosingPhrase, Submissions, Voting, Ended };
  public State GameState { get; set; }
  public Boolean Current { get; set; }
  public string? Phrase { get; set; }
  public DateTime StartedAt { get; set; }
  public List<Submission> Submissions { get; set; } = new List<Submission>();

  public Game()
  {
    Current = true;
  }

  public void Start()
  {
    GameState = State.ChoosingPhrase;
  }

  public void End()
  {
    GameState = State.Ended;
    Current = false;
  }

  public void SubmitPhrase(string text)
  {
    Phrase = text;
    GameState = State.Submissions;
  }

  public void SubmitGif(Guid playerId, string gifUrl)
  {
    bool submittedBefore = Submissions.Any(item => item.PlayerId == playerId);

    if (!submittedBefore)
    {
      Submission submission = new Submission { GifUrl = gifUrl, PlayerId = playerId };
      Submissions.Add(submission);
    }
  }

}

public class Submission
{
  public Guid PlayerId { get; set; }
  public string? GifUrl { get; set; }
  public DateTime submittedAt = DateTime.Now;
}
