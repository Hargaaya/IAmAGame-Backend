namespace IAmAGame_Backend.Utils;

public class KeyGenerator
{
    public string GenerateRoomKey()
    {
        string key = "";
        Random random = new Random();
        for (int i = 0; i < 5; i++)
        {
            int randomInt = random.Next(0, 2);
            if (randomInt == 0)
            {
                key += (char)random.Next(65, 91);
            }
            else
            {
                key += (char)random.Next(97, 123);
            }
        }
        return key;
    }
}

