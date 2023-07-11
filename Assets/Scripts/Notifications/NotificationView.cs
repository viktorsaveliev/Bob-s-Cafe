using DG.Tweening;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

public class NotificationView : MonoBehaviour
{
    [Header("Dialog")]

    [SerializeField] private GameObject _dialogWindow;
    [SerializeField] private Image _dialogBackground;

    [SerializeField] private TMP_Text _dialogHeader;
    [SerializeField] private TMP_Text _dialogText;

    [SerializeField] private Button _confirmDialogButton;
    [SerializeField] private Button _cancelDialogButton;

    [Header("Simple")]

    [SerializeField] private Image _simpleNotice;
    [SerializeField] private Image _simpleNoticeIcon;
    [SerializeField] private TMP_Text _simpleText;

    [SerializeField] private float _durationForAnimateSimpleNotice = 0.5f;

    public Action OnClickConfirm;
    public Action OnClickCancel;
    
    private bool _isSimpleNoticeShowed;
    private Coroutine _simpleNoticeTimer;

    private LocalizeStringEvent _dialogHeaderLocalize;
    private LocalizeStringEvent _dialogTextLocalize;
    private LocalizeStringEvent _simpleTextLocalize;

    private void Awake()
    {
        _dialogHeaderLocalize = _dialogHeader.GetComponent<LocalizeStringEvent>();
        _dialogTextLocalize = _dialogText.GetComponent<LocalizeStringEvent>();
        _simpleTextLocalize = _simpleText.GetComponent<LocalizeStringEvent>();
    }

    private void OnEnable()
    {
        _confirmDialogButton.onClick.AddListener(OnConfirmButtonClicked);
        _cancelDialogButton.onClick.AddListener(OnCancelButtonClicked);
    }

    private void OnDisable()
    {
        _confirmDialogButton.onClick.RemoveListener(OnConfirmButtonClicked);
        _cancelDialogButton.onClick.RemoveListener(OnCancelButtonClicked);
    }
    
    #region Dialog UI

    public void OnConfirmButtonClicked()
    {
        OnClickConfirm?.Invoke();
        HideDialog();
    }

    public void OnCancelButtonClicked()
    {
        OnClickCancel?.Invoke();
        HideDialog();
    }

    public void ShowDialog(string headerKey, string textKey)
    {
        _dialogHeaderLocalize.SetEntry(headerKey);
        _dialogTextLocalize.SetEntry(textKey);

        ShowDialogAnimation();
    }

    private void HideDialog()
    {
        HideDialogAnimation();
    }

    private void ShowDialogAnimation()
    {
        _dialogBackground.gameObject.SetActive(true);

        Color targetColor = new(_dialogBackground.color.r, _dialogBackground.color.g, _dialogBackground.color.b, 0.6f);
        _dialogBackground.color = new Color(_dialogBackground.color.r, _dialogBackground.color.g, _dialogBackground.color.b, 0);
        _dialogBackground.DOColor(targetColor, 0.3f);

        _dialogWindow.transform.localScale = Vector3.zero;
        _dialogWindow.SetActive(true);
        _dialogWindow.transform.DOScale(1f, 0.3f).SetEase(Ease.OutBack);
    }

    private void HideDialogAnimation()
    {
        _dialogBackground.gameObject.SetActive(false);

        _dialogWindow.transform.DOScale(0f, 0.3f).SetEase(Ease.InBack).OnComplete(() =>
        {
            _dialogWindow.SetActive(false);
        });
    }

    #endregion

    #region Simple UI

    public void ShowSimple(string localizeKey, int secondsToView)
    {
        if (_simpleNoticeTimer != null || _isSimpleNoticeShowed)
        {
            HideSimple(false);
        }

        _simpleTextLocalize.SetEntry(localizeKey);

        _simpleNotice.rectTransform.anchoredPosition = new Vector2(-600, -183);
        _simpleNotice.gameObject.SetActive(true);

        _simpleNotice.rectTransform
            .DOAnchorPosX(0, _durationForAnimateSimpleNotice)
            .SetEase(Ease.OutBack);

        _isSimpleNoticeShowed = true;

        _simpleNoticeTimer = StartCoroutine(TimerToHideSimpleNotice(secondsToView));
    }

    private void HideSimple(bool animate = true)
    {
        if(animate)
        {
            _simpleNotice.rectTransform
            .DOAnchorPosX(-600, _durationForAnimateSimpleNotice)
            .SetEase(Ease.InBack)
            .OnComplete(() =>
            {
                _simpleNotice.gameObject.SetActive(false);
            });
        }
        else
        {
            _simpleNotice.rectTransform.anchoredPosition = new Vector2(-600, -183);
            _simpleNotice.gameObject.SetActive(false);
        }

        _isSimpleNoticeShowed = false;

        if(_simpleNoticeTimer != null)
        {
            StopCoroutine(_simpleNoticeTimer);
            _simpleNoticeTimer = null;
        }
    }

    private IEnumerator TimerToHideSimpleNotice(int seconds)
    {
        yield return new WaitForSeconds(seconds);
        HideSimple();
    }

    #endregion
}
