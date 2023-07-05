using UnityEngine;

public class GameSettings : MonoBehaviour
{
    [SerializeField] private GameDataConfig _difficultyLevel;
    public GameDataConfig CurrentDifficultyLevel => _difficultyLevel;

    private readonly GameData _gameData = new();

    private void Awake()
    {
        _gameData.Init(this);
    }
}
