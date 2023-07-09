using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Shop : MonoBehaviour
{
    [SerializeField] private RectTransform[] _contentContainers;
    [SerializeField] private ScrollRect _viewport;
    [SerializeField] private GameObject _openShopButton;

    private ContentType _currentContentType;

    private enum ContentType
    {
        Tables,
        Decorations
    }

    private void Start()
    {
        ChangeContentType(0);
    }

    private void OnEnable()
    {
        EventBus.OnNewDayStarted += InactiveButtonForOpenShop;
        EventBus.OnCurrentDayEnded += ActiveButtonForOpenShop;
    }

    private void OnDisable()
    {
        EventBus.OnNewDayStarted -= InactiveButtonForOpenShop;
        EventBus.OnCurrentDayEnded -= ActiveButtonForOpenShop;
    }

    public void ChangeContentType(int typeID)
    {
        ContentType type = (ContentType)typeID;
        if (_currentContentType == type) return;

        _contentContainers[(int)_currentContentType].gameObject.SetActive(false);
        _contentContainers[typeID].gameObject.SetActive(true);
        _currentContentType = type;

        _viewport.content = _contentContainers[typeID];
    }

    public void ShowShop()
    {
        if (_viewport.gameObject.activeSelf) return;

        _viewport.transform.localScale = Vector3.zero;
        _viewport.gameObject.SetActive(true);
        _viewport.transform.DOScale(1, 0.3f).SetEase(Ease.OutBack);
    }

    public void HideShop()
    {
        if (_viewport.gameObject.activeSelf == false) return;

        _viewport.transform.DOScale(0, 0.3f).SetEase(Ease.InBack)
            .OnComplete(() => _viewport.gameObject.SetActive(false));
    }

    private void ActiveButtonForOpenShop()
    {
        _openShopButton.SetActive(true);
    }

    private void InactiveButtonForOpenShop()
    {
        _openShopButton.SetActive(false);
        HideShop();
    }
}
