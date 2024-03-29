﻿using IAmAGame_Backend.Engine.GifParty;
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

  public void StartGame(string key)
  {
    try
    {
      var game = _db.Get<GameRoom>(key);

      game.Start();

      _db.Set(key, game);

      Clients.Group(key).SendAsync("ReceiveGame", game);
    }
    catch (NullReferenceException ex)
    {
      Console.WriteLine($"GifPartyHub.StartGame: {ex.Message}");
    }
  }

  public void EndGame(string key)
  {
    try
    {
      var game = _db.Get<GameRoom>(key);

      game.Players.ForEach(p => p.HasVoted = false);
      //give points to winner
      var currentRoom = game.Games.Find(g => g.Current);

      if (currentRoom == null)
        throw new NullReferenceException("Current room is null");

      currentRoom.GameState = Game.State.Ended;

      // find submission with most votes
      var winnerSubmission = currentRoom.Submissions.OrderByDescending(s => s.Votes).First();
      // find player with that submission
      var winner = game.Players.Find(p => p.Id == winnerSubmission.PlayerId);

      if (winner == null)
        throw new NullReferenceException("Winner is null");

      winner.Score += 100;

      _db.Set(key, game);

      Clients.Group(key).SendAsync("ReceiveGame", game);
    }
    catch (NullReferenceException ex)
    {
      Console.WriteLine($"GifPartyHub.EndGame: {ex.Message}");
    }
  }

  public void EndGameRoom(string key)
  {
    try
    {
      var game = _db.Get<GameRoom>(key);

      var currentRoom = game.Games.Find(g => g.Current);

      if (currentRoom == null)
        throw new NullReferenceException("Current room is null");

      currentRoom.GameState = Game.State.Ended;

      game.End();

      _db.Set(key, game);

      Clients.Group(key).SendAsync("ReceiveGame", game);
    }
    catch (NullReferenceException ex)
    {
      Console.WriteLine($"GifPartyHub.EndGameRoom: {ex.Message}");
    }
  }

  public void StartVoting(string key)
  {
    try
    {
      var game = _db.Get<GameRoom>(key);
      var currentRoom = game.Games.Find(g => g.Current);

      if (currentRoom == null)
        throw new NullReferenceException("Current room is null");

      currentRoom.GameState = Game.State.Voting;

      _db.Set(key, game);

      Clients.Group(key).SendAsync("ReceiveGame", game);
    }
    catch (NullReferenceException ex)
    {
      Console.WriteLine($"GifPartyHub.StartVoting: {ex.Message}");
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

      player.ConnectionId = Context.ConnectionId;
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

      Clients.Group(key).SendAsync("ReceiveGame", game);
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

      Groups.RemoveFromGroupAsync(player.ConnectionId, key);

      Clients.Client(Context.ConnectionId).SendAsync("ReceivePlayer", player);

      Groups.AddToGroupAsync(Context.ConnectionId, key);
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
      Console.WriteLine($"GifPartyHub.SetPlayerReadyState: {ex.Message}");
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

  public void SetPhrase(string key, string phrase)
  {
    try
    {
      var game = _db.Get<GameRoom>(key);
      var currentRoom = game.Games.Find(g => g.Current);

      if (currentRoom == null)
        throw new NullReferenceException("Current room is null");

      currentRoom.Phrase = phrase;
      currentRoom.GameState = Game.State.Submissions;

      _db.Set(key, game);

      Clients.Group(key).SendAsync("ReceiveGame", game);

      Console.WriteLine($"SetPhrase: {phrase} for room {game.Name}");
    }
    catch (NullReferenceException ex)
    {
      Console.WriteLine($"GifPartyHub.SetPhrase: {ex.Message}");
    }
  }

  public void AddSubmission(string key, Player player, string gifUrl)
  {
    try
    {
      var game = _db.Get<GameRoom>(key);
      var currentRoom = game.Games.Find(g => g.Current);

      if (currentRoom == null)
        throw new NullReferenceException("Current room is null");

      currentRoom.SubmitGif(player.Id, gifUrl);

      _db.Set(key, game);

      Clients.Group(key).SendAsync("ReceiveGame", game);

      Console.WriteLine($"AddSubmission: {gifUrl} for room {game.Name} by {player.Name}");
    }
    catch (NullReferenceException ex)
    {
      Console.WriteLine($"GifPartyHub.AddSubmission: {ex.Message}");
    }
  }

  public void Vote(string key, Player player, Guid votedOnPlayerId)
  {
    try
    {
      var game = _db.Get<GameRoom>(key);
      var currentRoom = game.Games.Find(g => g.Current);

      var playerWhoIsVoting = game.Players.Find(p => p.Id == player.Id);

      if (currentRoom == null)
        throw new NullReferenceException("Current room is null");

      var submission = currentRoom.Submissions.Find(s => s.PlayerId == votedOnPlayerId);

      if (submission == null)
        throw new NullReferenceException("Submission is null");

      if (playerWhoIsVoting == null)
        throw new NullReferenceException("Player is not in game");

      if (playerWhoIsVoting.HasVoted)
        throw new Exception("Player has already voted");

      playerWhoIsVoting.HasVoted = true;
      submission.Votes++;

      _db.Set(key, game);

      Clients.Group(key).SendAsync("ReceiveGame", game);
      Clients.Client(Context.ConnectionId).SendAsync("ReceivePlayer", playerWhoIsVoting);

      Console.WriteLine($"Voted on: {votedOnPlayerId} for room {game.Name} by {player.Name}");
    }
    catch (NullReferenceException ex)
    {
      Console.WriteLine($"GifPartyHub.Vote: {ex.Message}");
    }
  }
}
