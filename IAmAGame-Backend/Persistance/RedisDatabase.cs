using StackExchange.Redis;
using System.Text.Json;

namespace IAmAGame_Backend.Persistance;

public class RedisDatabase
{
    private IDatabase db;

    public RedisDatabase()
    {
        ConnectionMultiplexer redis = ConnectionMultiplexer.Connect("localhost");
        db = redis.GetDatabase();
    }

    public void Set<T>(string key, T data)
    {
        string value = JsonSerializer.Serialize(data);
        db.StringSet(key, value);
    }

    public T? Get<T>(string key)
    {
        string? value = db.StringGet(key);
        if (value == null) return default;
        T? data = JsonSerializer.Deserialize<T>(value);

        return data;
    }

}
