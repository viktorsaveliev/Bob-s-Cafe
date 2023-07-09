using UnityEngine;

public class WallpaperChanger : IShopItemObserver
{
    private readonly MeshRenderer[] _walls;

    private Material _currentWallpapers;
    private Material _previewWallpapers;

    public WallpaperChanger(MeshRenderer[] walls)
    {
        _walls = walls;
    }

    private void ChangeWallpaper(Material wallpaper, int price)
    {
        if (_previewWallpapers == wallpaper || _currentWallpapers == wallpaper) return;

        foreach(MeshRenderer wall in _walls)
        {
            wall.material = wallpaper;
        }

        _previewWallpapers = wallpaper;
    }

    public void OnShopItemClicked<T>(T prefab, int price)
    {
        var material = prefab as Material;
        ChangeWallpaper(material, price);
    }
}
