using UnityEngine;
using DG.Tweening;

public class ObjectDragController : MonoBehaviour, IObjectDragController
{
    [SerializeField] private LayerMask _floorLayer;
    [SerializeField] private LayerMask _wallsLayer;

    [SerializeField] private Material[] _transperentMaterial;
    [SerializeField] private ParticleSystem _buyFX;

    private GameObject _transparentObject;
    private GameObject _itemPrefab;
    private int _price;

    public enum FixedOn
    {
        Floor,
        Walls
    }

    private void OnEnable()
    {
        EventBus.OnPlayerSelectShopItem += CreateDraggableObject;
    }

    private void OnDisable()
    {
        EventBus.OnPlayerSelectShopItem -= CreateDraggableObject;
    }

    public void CreateDraggableObject(GameObject itemPrefab, int price)
    {
        if (_transparentObject != null) DestroyDraggableObject();

        _itemPrefab = itemPrefab;
        _price = price;

        _transparentObject = Instantiate(itemPrefab, itemPrefab.transform.position + Vector3.up * 0.5f, itemPrefab.transform.rotation);

        DraggableObject draggableObject = _transparentObject.AddComponent<DraggableObject>();
        if(itemPrefab.GetComponent<Table>() != null)
        {
            draggableObject.Init(this, _floorLayer, FixedOn.Floor);
        }
        else
        {
            draggableObject.Init(this, _wallsLayer, FixedOn.Walls);
        }
    }

    public void DestroyDraggableObject()
    {
        Destroy(_transparentObject);
    }

    public void SpawnItem(bool isAllowed)
    {
        if (isAllowed == false)
        {
            print("Wrong location");
            return;
        }

        if (Money.Spend(_price) == false) return;

        GameObject item = Instantiate(_itemPrefab, _transparentObject.transform.position, _transparentObject.transform.rotation);
        Transform itemTransform = item.transform;

        Vector3 standartScale = itemTransform.localScale;
        itemTransform.localScale = Vector3.zero;
        itemTransform.DOScale(standartScale, 0.4f).SetEase(Ease.OutBack).OnComplete(() =>
        {
            EventBus.OnPlayerBoughtShopItem?.Invoke();
        });

        PlayPurchaseFX(itemTransform.position);
        DestroyDraggableObject();
    }

    public Material[] GetObjectStatusMaterials()
    {
        return _transperentMaterial;
    }

    private void PlayPurchaseFX(Vector3 position)
    {
        _buyFX.transform.position = position;
        _buyFX.Play();
    }
}