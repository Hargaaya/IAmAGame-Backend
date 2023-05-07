using StackExchange.Redis;
using System.Text.Json;

namespace IAmAGame_Backend.Persistance;

public class RedisDatabase : IRedisDatabase
{
    private IDatabase db;

    public RedisDatabase()
    {
        // TODO: Move out db connection string to a config file
        ConnectionMultiplexer redis = ConnectionMultiplexer.Connect("localhost");
        db = redis.GetDatabase();
    }

    public void Set<T>(string key, T data)
    {
        string value = JsonSerializer.Serialize(data);
        db.StringSet(key, value);
    }

    public T Get<T>(string key)
    {
        string nullMessage = $"Data for game with key: {key}, is null";

        string? value = db.StringGet(key);
        _ = value ?? throw new NullReferenceException(nullMessage);

        T? data = JsonSerializer.Deserialize<T>(value);
        _ = data ?? throw new NullReferenceException(nullMessage);

        return data;
    }

}
