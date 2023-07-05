using UnityEngine;

[CreateAssetMenu(fileName = "GameDataConfig", menuName = "Game/GameDataConfig")]
public class GameDataConfig : ScriptableObject
{
    [SerializeField] private string _difficultyLevel;
    [SerializeField] private int _moneyForHapinessVisitor;
    [SerializeField] private int _moneyForDissatisfiedVisitor;
    [SerializeField] private int _penaltyForDepartedVisitor;
    
    public string DifficultyLevel => _difficultyLevel;
    public int MoneyForHapinessVisitor => _moneyForHapinessVisitor;
    public int MoneyForDissatisfiedVisitor => _moneyForDissatisfiedVisitor;
    public int PenaltyForDepartedVisitor => _penaltyForDepartedVisitor;
}
