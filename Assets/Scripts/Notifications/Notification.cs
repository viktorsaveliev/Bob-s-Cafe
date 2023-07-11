using System;

public class Notification
{
    public enum MessageType
    {
        SaleFurniture,
        DontHaveMoney,
        NoEmptySeats,
        IncorrectValue,
        VisitorWantsSeparately
    }

    private static NotificationController _notificationController;

    public Notification(NotificationController notificationController)
    {
        _notificationController = notificationController;
    }

    public static void ShowDialog(MessageType messageType, Action onConfirm, Action onCancel = null)
    {
        if (_notificationController == null) return;

        NotificationData data = new(_notificationController.LocalizeKeys[(int)messageType, 0], _notificationController.LocalizeKeys[(int)messageType, 1])
        {
            OnConfirm = onConfirm,
            OnCancel = onCancel
        };

        _notificationController.ShowDialog(data);
    }

    public static void ShowSimple(MessageType messageType, int secondsToView = 3)
    {
        if (_notificationController == null) return;

        _notificationController.ShowSimple(_notificationController.LocalizeKeys[(int)messageType, 1], secondsToView);
    }
}