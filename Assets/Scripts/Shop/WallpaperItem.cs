using UnityEngine;

public class WallpaperItem : ShopItem
{
    [SerializeField] private Material _wallpaperMaterial;

    public override void SelectItem()
    {
        if (Money.Value < Price) return;

        Observer?.OnShopItemClicked(_wallpaperMaterial, Price);
    }
}
