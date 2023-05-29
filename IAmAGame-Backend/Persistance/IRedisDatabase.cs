namespace IAmAGame_Backend.Persistance;

public interface IRedisDatabase
{
    public void Set<T>(string key, T data);
    public T Get<T>(string key);
}

