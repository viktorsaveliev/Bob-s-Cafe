using System;
using UnityEngine;

[RequireComponent(typeof(NotificationController))]
public class Notification : MonoBehaviour
{
    private static NotificationController _notificationController;

    private void Awake()
    {
        _notificationController = GetComponent<NotificationController>();
    }

    public static void Show(string keyHeader, string keyText, Action onConfirm = null, Action onCancel = null)
    {
        if (_notificationController == null) return;

        NotificationData data = new(keyHeader, keyText)
        {
            OnConfirm = onConfirm,
            OnCancel = onCancel
        };

        _notificationController.ShowNotification(data);
    }
}