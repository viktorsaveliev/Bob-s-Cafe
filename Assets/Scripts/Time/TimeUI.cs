using UnityEngine;
using DG.Tweening;
using TMPro;

[RequireComponent(typeof(TimeController))]
public class TimeUI : MonoBehaviour, IStartDayObserver, IEndDayObserver
{
    [SerializeField] private TMP_Text _timeText;
    [SerializeField] private TMP_Text _dayText;
    [SerializeField] private GameObject _buttonForOpenCafe;

    private TimeController _time;

    private void Awake()
    {
        _time = GetComponent<TimeController>();
    }

    private void OnEnable()
    {
        _time.SubscribeToMinutePassed(UpdateUI);
    }

    private void OnDisable()
    {
        _time.UnsubscribeFromMinutePassed(UpdateUI);
    }

    private void UpdateUI()
    {
        _timeText.text = _time.GetCurrentTime();
    }

    public void OnDayStarted()
    {
        _dayText.text = $"Day {_time.GetCurrentDay}";
        _dayText.transform.localScale = Vector3.zero;
        _dayText.rectTransform.anchoredPosition = new Vector2(-100f, 0);
        _dayText.gameObject.SetActive(true);

        _dayText.transform.DOScale(1f, 0.5f).SetEase(Ease.OutBack);
        _dayText.rectTransform.DOAnchorPosX(20f, 3f).OnComplete(() =>
        {
            _dayText.transform.DOScale(0f, 0.5f).SetEase(Ease.InBack).OnComplete(() => _dayText.gameObject.SetActive(false));
        });
    }

    public void OnDayEnded()
    {
        _buttonForOpenCafe.transform.localScale = Vector3.zero;
        _buttonForOpenCafe.SetActive(true);
        _buttonForOpenCafe.transform.DOScale(1f, 1f);
    }
}
