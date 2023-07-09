using UnityEngine;

[CreateAssetMenu(fileName = "GameDataConfig", menuName = "Game/GameDataConfig")]
public class GameDataConfig : ScriptableObject
{
    [SerializeField] private string _difficultyLevelTitle = "Normal";

    [SerializeField, Range(1, 100)] private int _moneyForHapinessVisitor;
    [SerializeField, Range(1, 100)] private int _moneyForDissatisfiedVisitor;
    [SerializeField, Range(1, 100)] private int _penaltyForDepartedVisitor;

    [SerializeField, Range(1, 1000)] private int _priceForTable;
    [SerializeField, Range(1, 1000)] private int _priceForChair;

    [SerializeField, Range(1, 1000)] private int _priceForPainting;
    
    public string DifficultyLevelTitle => _difficultyLevelTitle;
    public int MoneyForHapinessVisitor => _moneyForHapinessVisitor;
    public int MoneyForDissatisfiedVisitor => _moneyForDissatisfiedVisitor;
    public int PenaltyForDepartedVisitor => _penaltyForDepartedVisitor;

    public int PriceForTable => _priceForTable;
    public int PriceForChair => _priceForChair;
    public int PriceForPainting => _priceForPainting;
}
