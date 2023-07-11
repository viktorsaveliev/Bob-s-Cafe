using System;

public class NotificationController
{
    public string[,] LocalizeKeys => _localizeKeys;

    private readonly string[,] _localizeKeys =
    {
        {"SaleFurniture_Header",        "SaleFurniture_Text"},
        {"DontHaveMoney_Header",        "DontHaveMoney_Text"},
        {"NoEmptySeats_Header",         "NoEmptySeats_Text" },
        {"IncorrectValue_Header",       "IncorrectValue_Text"},
        {"VisitorSeparately_Header",    "VisitorSeparately_Text"}
    };

    private readonly NotificationView _notificationView;

    private Action _onConfirm;
    private Action _onCancel;

    public NotificationController(NotificationView notificationView)
    {
        _notificationView = notificationView;
    }

    public void Init()
    {
        _notificationView.OnClickConfirm += OnConfirmDialogButtonClicked;
        _notificationView.OnClickCancel += OnCancelDialogButtonClicked;
    }

    public void DeInit()
    {
        _notificationView.OnClickConfirm -= OnConfirmDialogButtonClicked;
        _notificationView.OnClickCancel -= OnCancelDialogButtonClicked;
    }

    public void OnConfirmDialogButtonClicked()
    {
        _onConfirm?.Invoke();
    }

    public void OnCancelDialogButtonClicked()
    {
        _onCancel?.Invoke();
    }

    public void ShowDialog(NotificationData data)
    {
        _onConfirm = data.OnConfirm;
        _onCancel = data.OnCancel;

        _notificationView.ShowDialog(data.Header, data.Text);
    }

    public void ShowSimple(string localizeKey, int secondsToView)
    {
        _notificationView.ShowSimple(localizeKey, secondsToView);
    }
}
