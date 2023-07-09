using UnityEngine;
using Zenject;

public class NotificationInstaller : MonoInstaller
{
    [SerializeField] private NotificationController _notificationController;

    public override void InstallBindings()
    {
        Container.Bind<NotificationController>().FromInstance(_notificationController).AsSingle();
    }
}
