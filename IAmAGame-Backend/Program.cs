using IAmAGame_Backend.Engine.GifParty;
using IAmAGame_Backend.Persistance;
using IAmAGame_Backend.Utils;

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();
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