using UnityEngine;
using UnityEngine.UI;
using TMPro;

public abstract class ShopItem : MonoBehaviour
{
    [SerializeField] private TMP_Text _nameText;
    [SerializeField] private string _name;

    [SerializeField] private TMP_Text _priceText;
    [SerializeField] protected int Price;

    private Button _button;

    private void Awake()
    {
        Init();
    }

    private void OnEnable()
    {
        UpdatePriceUI();
        EventBus.OnMoneyValueChanged += UpdatePriceUI;
    }

    private void OnDisable()
    {
        EventBus.OnMoneyValueChanged -= UpdatePriceUI;
    }

    public virtual void Init()
    {
        _nameText.text = _name;
        _priceText.text = $"{Price}$";
        _button = GetComponent<Button>();
    }

    public virtual void SelectItem() { }

    private void UpdatePriceUI()
    {
        if (Money.Value < Price)
        {
            _button.interactable = false;
            _priceText.color = Color.red;
        }
        else
        {
            _button.interactable = true;
            _priceText.color = Color.green;
        }
    }
}
