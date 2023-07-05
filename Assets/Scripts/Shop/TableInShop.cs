using UnityEngine;

public class TableInShop : ShopItem
{
    [SerializeField] private GameObject _itemPrefab;

    public override void SelectItem()
    {
        base.SelectItem();

        if (Money.Value < Price) return;
        EventBus.OnPlayerSelectShopItem?.Invoke(_itemPrefab, Price);
    }
}
