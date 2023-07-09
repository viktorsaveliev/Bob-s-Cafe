using UnityEngine;
using Zenject;

public class Bootstrap : MonoBehaviour
{
    [Header("Visitors Data")]

    [SerializeField] private Groups _groups;
    [SerializeField] private VisitorsQueue _visitorsQueue;
    [SerializeField] private VisitorsSpawner _visitorSpawner;

    [Header("Facades")]

    [SerializeField] private Money _moneyFacade;

    [Header("Furniture & Shop")]

    [SerializeField] private MeshRenderer[] _walls;
    [SerializeField] private FurnitureUI _furnitureUI;

    [Inject] private readonly FurniturePlacemant _furniturePlacemant;

    private FurnitureController _furnitureController;
    private WallpaperChanger _wallpaperChanger;

    private void Awake()
    {
        _groups.Init();
        _visitorsQueue.Init();
        _visitorSpawner.Init();

        _moneyFacade.Init();

        _furnitureController = new(_furniturePlacemant, _furnitureUI);
        _furnitureController.Init();

        InitShopItems();

    }

    private void InitShopItems()
    {
        _wallpaperChanger = new(_walls);

        ShopItem[] shopItems = FindObjectsOfType<ShopItem>(true);

        foreach(ShopItem item in shopItems)
        {
            item.Init();

            if(item is FurnitureShopItem)
            {
                item.SetObserver(_furniturePlacemant);
            }
            else
            {
                item.SetObserver(_wallpaperChanger);
            }
        }

        UpdateShopPriceOnGameStarted updateFurniturePrice = new();
        updateFurniturePrice.Update();
    }
}
