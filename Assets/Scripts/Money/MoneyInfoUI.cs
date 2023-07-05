using UnityEngine;
using UnityEngine.UI;

public class MoneyInfoUI : MonoBehaviour
{
    [SerializeField] private Text _moneyText;

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
