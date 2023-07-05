using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

[RequireComponent(typeof(TimeControl))]
public class TimeUI : MonoBehaviour
{
    [SerializeField] private Text _timeText;
    [SerializeField] private Text _dayText;
    [SerializeField] private GameObject _buttonForOpenCafe;

    private TimeControl _time;

    private void Awake()
    {
        _time = GetComponent<TimeControl>();
    }

    private void OnEnable()
    {
        EventBus.OnNewDayStarted += OnDayStarted;
        EventBus.OnCurrentDayEnded += OnDayEnded;

        _time.SubscribeToMinutePassed(UpdateUI);
    }

    private void OnDisable()
    {
        EventBus.OnNewDayStarted -= OnDayStarted;
        EventBus.OnCurrentDayEnded -= OnDayEnded;

        _time.UnsubscribeFromMinutePassed(UpdateUI);
    }

    private void UpdateUI()
    {
        _timeText.text = _time.GetCurrentTime();
    }

    private void OnDayStarted()
    {
        _dayText.text = $"Day {_time.GetCurrendDay}";
        _dayText.transform.localScale = Vector3.zero;
        _dayText.rectTransform.anchoredPosition = new Vector2(-100f, 0);
        _dayText.gameObject.SetActive(true);

        _dayText.transform.DOScale(1f, 0.5f).SetEase(Ease.OutBack);
        _dayText.rectTransform.DOAnchorPosX(20f, 3f).OnComplete(() =>
        {
            _dayText.transform.DOScale(0f, 0.5f).SetEase(Ease.InBack).OnComplete(() => _dayText.gameObject.SetActive(false));
        });
    }

    private void OnDayEnded()
    {
        _buttonForOpenCafe.transform.localScale = Vector3.zero;
        _buttonForOpenCafe.SetActive(true);
        _buttonForOpenCafe.transform.DOScale(1f, 1f);
    }
}
