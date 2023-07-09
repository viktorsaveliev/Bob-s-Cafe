using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(Button))]
public abstract class ShopItem : MonoBehaviour
{
    [SerializeField] protected int Price;
    [SerializeField] private TMP_Text _nameText;
    [SerializeField] private TMP_Text _priceText;
    [SerializeField] private string _name;

    protected IShopItemObserver Observer;
    private Button _button;

    private void OnEnable()
    {
        UpdateButtonInteractable();
        EventBus.OnMoneyValueChanged += UpdateButtonInteractable;

        _button.onClick.AddListener(SelectItem);
    }

    private void OnDisable()
    {
        EventBus.OnMoneyValueChanged -= UpdateButtonInteractable;

        _button.onClick.RemoveListener(SelectItem);
    }

    public void SetPrice(int price)
    {
        if (price < 1) return;

        Price = price;
        UpdatePriceUI();
    }

    public void SetObserver(IShopItemObserver observer)
    {
        Observer = observer;
    }

    public virtual void Init()
    {
        _nameText.text = _name;
        _priceText.text = $"{Price}$";
        _button = GetComponent<Button>();
    }

    public abstract void SelectItem();

    private void UpdateButtonInteractable()
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

    private void UpdatePriceUI() => _priceText.text = $"{Price}$";
}
