
public class GameData
{
    public static GameSettings Settings { get; private set; }
    
    public void Init(GameSettings gameSettings)
    {
        Settings = gameSettings;
    }
}
