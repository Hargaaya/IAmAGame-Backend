using IAmAGame_Backend.Engine.GifParty;
using IAmAGame_Backend.Hubs;
using IAmAGame_Backend.Persistance;
using IAmAGame_Backend.Utils;

string myAllowSpecificOrigins = "_myAllowSpecificOrigins";

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

string originsString = builder.Configuration["AllowedOrigins"] ?? "*";
builder.Services.AddCors(opt =>
{
  opt.AddPolicy(name: myAllowSpecificOrigins, policyBuilder =>
  {
    policyBuilder.WithOrigins("http://127.0.0.1:5173")
          .AllowCredentials()
          .AllowAnyHeader()
          .AllowAnyMethod()
          .SetIsOriginAllowed((host) => true);
  });
});

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSignalR();


WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
}

app.UseRouting();
app.UseCors(myAllowSpecificOrigins);

app.MapHub<GifPartyHub>("/hub/gifparty");

app.MapPost("/room/{gameType}", (string gameType) =>
{
  // TODO: Switch statement once we get one more game up.
  if (gameType == "gif-party")
  {
    // TODO: Dependency Inject this instead, after getting started.
    var db = new RedisDatabase();

    var key = new KeyGenerator().GenerateRoomKey();
    var game = new GameRoom(key);

    db.Set(key, game);

    return key;
  }

  return null;
});

app.Run();