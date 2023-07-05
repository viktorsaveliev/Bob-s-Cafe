using UnityEngine;

public class Bootstrap : MonoBehaviour
{
    [SerializeField] private Groups _groups;
    [SerializeField] private VisitorsQueue _visitorsQueue;
    [SerializeField] private VisitorsSpawner _visitorSpawner;

    [Header("Facades")]
    [SerializeField] private Money _moneyFacade;

    private void Awake()
    {
        _groups.Init();
        _visitorsQueue.Init();
        _visitorSpawner.Init();

        _moneyFacade.Init();
    }
}
