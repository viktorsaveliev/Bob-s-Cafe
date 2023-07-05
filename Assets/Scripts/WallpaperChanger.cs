using UnityEngine;

public class WallpaperChanger : MonoBehaviour
{
    [SerializeField] private MeshRenderer[] _walls;
    private Material _currentWallpapers;

    private void OnEnable()
    {
        EventBus.OnPlayerSelectWallpaper += ChangeWallpaper;
    }

    private void OnDisable()
    {
        EventBus.OnPlayerSelectWallpaper -= ChangeWallpaper;
    }

    private void ChangeWallpaper(Material wallpaper, int price)
    {
        if (_currentWallpapers == wallpaper) return;

        foreach(MeshRenderer wall in _walls)
        {
            wall.material = wallpaper;
        }

        _currentWallpapers = wallpaper;
    }
}
