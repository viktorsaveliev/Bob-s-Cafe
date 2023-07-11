using UnityEngine;
using TMPro;

public class MoneyInfoUI : MonoBehaviour
{
    [SerializeField] private TMP_Text _moneyText;

    private void OnEnable()
    {
        EventBus.OnMoneyValueChanged += UpdateMoneyValueUI;
    }

    private void OnDisable()
    {
        EventBus.OnMoneyValueChanged -= UpdateMoneyValueUI;
    }

    private void UpdateMoneyValueUI()
    {
        _moneyText.text = $"{Money.Value}$";
    }
}
