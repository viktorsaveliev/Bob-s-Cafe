using UnityEngine;
using Zenject;

public class Bootstrap : MonoBehaviour
{
    [Header("Time flow")]

    [SerializeField] private TimeController _timeController;
    [SerializeField] private TimeUI _timeUI;
    [SerializeField] private SunMovement _sunMovement;

    [Header("Visitors Data")]

    [SerializeField] private Groups _groups;
    [SerializeField] private VisitorsQueue _visitorsQueue;
    [SerializeField] private VisitorsSpawner _visitorSpawner;

    [Header("Facades")]

    [SerializeField] private Money _moneyFacade;
    private Notification _notificationFacade;

    [Header("Furniture & Shop")]

    [SerializeField] private Shop _shop;
    [SerializeField] private MeshRenderer[] _walls;
    [SerializeField] private FurnitureUI _furnitureUI;

    [Inject] private readonly FurniturePlacemant _furniturePlacemant;

    private FurnitureController _furnitureController;
    private WallpaperChanger _wallpaperChanger;

    [Header("Notification")]

    [SerializeField] private NotificationView _notificationView;
    private NotificationController _notificationController;

    private void Awake()
    {
        InitVisitors();
        InitFurniture();
        InitShopItems();
        InitNotification();

        _moneyFacade.Init();

        InitTime();
    }

    private void InitTime()
    {
        _timeController.AddEndDayObserver(_visitorsQueue);
        _timeController.AddEndDayObserver(_furnitureController);
        _timeController.AddEndDayObserver(_timeUI);
        _timeController.AddEndDayObserver(_shop);
        _timeController.AddEndDayObserver(_sunMovement);
        _timeController.AddEndDayObserver(_visitorSpawner);

        _timeController.AddStartDayObserver(_furnitureController);
        _timeController.AddStartDayObserver(_timeUI);
        _timeController.AddStartDayObserver(_shop);
        _timeController.AddStartDayObserver(_sunMovement);
        _timeController.AddStartDayObserver(_visitorSpawner);
    }

    private void InitVisitors()
    {
        _groups.Init();
        _visitorsQueue.Init();
        _visitorSpawner.Init();
    }

    private void InitFurniture()
    {
        _furnitureController = new(_furniturePlacemant, _furnitureUI);
        _furnitureController.Init();

        
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

    private void InitNotification()
    {
        _notificationController = new(_notificationView);
        _notificationFacade = new(_notificationController);

        _notificationController.Init();
    }
}