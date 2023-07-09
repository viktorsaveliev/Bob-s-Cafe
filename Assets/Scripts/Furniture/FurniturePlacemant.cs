using UnityEngine;
using DG.Tweening;

public class FurniturePlacemant : IShopItemObserver
{
    private readonly FurnitureFactory _furnitureFactory;

    private readonly Material[] _transperentMaterial;
    private readonly LayerMask _floorLayer;
    private readonly LayerMask _wallsLayer;

    private readonly ParticleSystem _buyFX;

    private GameObject _draggedObject;
    private GameObject _itemPrefab;
    private int _price;

    private Vector3 _startDragPosition;

    public FurniturePlacemant(FurnitureFactory furnitureFactory,
                                LayerMask floorLayer,
                                LayerMask wallsLayer,
                                Material[] transperentMaterial,
                                ParticleSystem buyFX)
    {
        _furnitureFactory = furnitureFactory;
        _floorLayer = floorLayer;
        _wallsLayer = wallsLayer;
        _transperentMaterial = transperentMaterial;
        _buyFX = buyFX;
    }

    public void CreateDraggableObject(GameObject itemPrefab, int price)
    {
        if (_draggedObject != null) DestroyDraggableObject();

        _startDragPosition = itemPrefab.transform.position;
        _itemPrefab = itemPrefab;
        _price = price;

        _draggedObject = Object.Instantiate(itemPrefab, Vector3.zero + Vector3.up * 0.5f, itemPrefab.transform.rotation);

        DraggableObject draggableObject = _draggedObject.AddComponent<DraggableObject>();
        bool isFurnitureOnFloor = itemPrefab.GetComponent<IFurnitureOnFloor>() != null;

        if (isFurnitureOnFloor)
        {
            draggableObject.Init(_floorLayer, _transperentMaterial, new DragOnFloor(draggableObject.transform, 1f));
        }
        else
        {
            draggableObject.Init(_wallsLayer, _transperentMaterial, new DragOnWalls(draggableObject.transform));
        }

        draggableObject.OnDragCompleted += SpawnItem;
        draggableObject.OnDragCanceled += DestroyDraggableObject;

        if (price == 0)
        {
            draggableObject.OnDragCanceled += ResetPosition;
        }
    }

    public void OnShopItemClicked<T>(T prefab, int price)
    {
        GameObject furniture = prefab as GameObject;
        if (furniture != null)
            CreateDraggableObject(furniture, price);
    }

    public void DestroyDraggableObject()
    {
        UnSubscribeFromAction();
        Object.Destroy(_draggedObject);
    }

    public void SpawnItem(bool isAllowed)
    {
        if (!isAllowed)
        {
            Debug.Log("Wrong location");
            return;
        }

        GameObject item;

        if (_price > 0) // buy
        {
            if (!TryPurchaseItem()) return;

            item = _furnitureFactory.CreateFurniture(_itemPrefab, _draggedObject.transform.position, _draggedObject.transform.rotation);
            item.GetComponent<Furniture>().SetPrice(_price);
            PlayPurchaseFX(item.transform.position);

            EventBus.OnPlayerBoughtShopItem?.Invoke();
        }
        else // edit
        {
            item = _itemPrefab;
            PlaceItemInEditMode();
        }

        AnimateSpawnedItem(item);
        DestroyDraggableObject();
    }

    private bool TryPurchaseItem() => Money.Spend(_price);

    private void PlaceItemInEditMode()
    {
        _itemPrefab.transform.position = _draggedObject.transform.position;
        _itemPrefab.SetActive(true);
    }

    private void AnimateSpawnedItem(GameObject item)
    {
        Transform itemTransform = item.transform;
        Vector3 standardScale = itemTransform.localScale;
        itemTransform.localScale = Vector3.zero;
        itemTransform.DOScale(standardScale, 0.4f).SetEase(Ease.OutBack);
    }

    private void PlayPurchaseFX(Vector3 position)
    {
        _buyFX.transform.position = position;
        _buyFX.Play();
    }

    private void UnSubscribeFromAction()
    {
        if (_draggedObject.TryGetComponent(out DraggableObject draggableObject))
        {
            draggableObject.OnDragCompleted -= SpawnItem;
            draggableObject.OnDragCanceled -= DestroyDraggableObject;

            if (_price == 0)
            {
                draggableObject.OnDragCanceled -= ResetPosition;
            }
        }
    }

    private void ResetPosition()
    {
        _itemPrefab.transform.position = _startDragPosition;
        _itemPrefab.SetActive(true);
    }
}