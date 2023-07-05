using UnityEngine;
using UnityEngine.UI;

public class WallpaperShopItem : ShopItem
{
    [SerializeField] private Material _wallpaperMaterial;

    public override void SelectItem()
    {
        base.SelectItem();

        if (Money.Value < Price) return;
        EventBus.OnPlayerSelectWallpaper?.Invoke(_wallpaperMaterial, Price);
    }
}
