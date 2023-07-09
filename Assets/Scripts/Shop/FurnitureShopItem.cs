using UnityEngine;

public class FurnitureShopItem : ShopItem
{
    [SerializeField] private GameObject _itemPrefab;
    public GameObject ItemPrefab => _itemPrefab;

    public override void SelectItem()
    {
        if (Money.Value < Price) return;

        Observer?.OnShopItemClicked(_itemPrefab, Price);
    }
}
