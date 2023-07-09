using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class NotificationController : MonoBehaviour
{
    [SerializeField] private GameObject _window;
    [SerializeField] private Image _background;

    [SerializeField] private TMP_Text _notificationHeader;
    [SerializeField] private TMP_Text _notificationText;
    
    [SerializeField] private Button _confirmButton;
    [SerializeField] private Button _cancelButton;

    private Action _onConfirm;
    private Action _onCancel;

    private void OnEnable()
    {
        _confirmButton.onClick.AddListener(OnConfirmButtonClicked);
        _cancelButton.onClick.AddListener(OnCancelButtonClicked);
    }

    private void OnDisable()
    {
        _confirmButton.onClick.RemoveListener(OnConfirmButtonClicked);
        _cancelButton.onClick.RemoveListener(OnCancelButtonClicked);
    }

    public void OnConfirmButtonClicked()
    {
        _onConfirm?.Invoke();
        HideNotification();
    }

    public void OnCancelButtonClicked()
    {
        _onCancel?.Invoke();
        HideNotification();
    }

    public void ShowNotification(NotificationData data)
    {
        _notificationHeader.text = data.Header;
        _notificationText.text = data.Text;
        
        _onConfirm = data.OnConfirm;
        _onCancel = data.OnCancel;

        ShowAnimation();
    }

    private void HideNotification()
    {
        HideAnimation();
    }

    private void ShowAnimation()
    {
        _background.gameObject.SetActive(true);

        Color targetColor = new(_background.color.r, _background.color.g, _background.color.b, 0.6f);
        _background.color = new Color(_background.color.r, _background.color.g, _background.color.b, 0);
        _background.DOColor(targetColor, 0.3f);

        _window.transform.localScale = Vector3.zero;
        _window.SetActive(true);
        _window.transform.DOScale(1f, 0.3f).SetEase(Ease.OutBack);
    }

    private void HideAnimation()
    {
        _background.gameObject.SetActive(false);

        _window.transform.DOScale(0f, 0.3f).SetEase(Ease.InBack).OnComplete(() =>
        {
            _window.SetActive(false);
        });
    }
}
