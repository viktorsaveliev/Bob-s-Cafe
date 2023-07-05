using UnityEngine;
using System.Collections;

[RequireComponent(typeof(VisitorsQueue))]
public class VisitorsSpawner : ObjectPool
{
    [SerializeField] private GameObject _prefab;
    [SerializeField] private int _maxVisitors = 10;

    private readonly VisitorStartWaiting _visitorStartWaiting = new();
    private readonly float _timeToCreateNewVisitor = 7f;
    public float GetTimeToCreateNewVisitor => _timeToCreateNewVisitor;

    private VisitorsQueue _visitorsQueue;
    private int GetFreeSlotsInQueue => _visitorsQueue.MAX_VISITORS_IN_QUEUE - _visitorsQueue.Queue.Count;

    private Groups _groups;
    private Coroutine _spawnTimer;


    private void OnEnable()
    {
        EventBus.OnNewDayStarted += StartSpawnVisitors;
        EventBus.OnCurrentDayEnded += StopSpawnVisitors;
    }

    private void OnDisable()
    {
        EventBus.OnNewDayStarted -= StartSpawnVisitors;
        EventBus.OnCurrentDayEnded -= StopSpawnVisitors;
    }

    public void Init()
    {
        _visitorsQueue = GetComponent<VisitorsQueue>();
        _groups = GetComponent<Groups>();

        Capacity = _maxVisitors;

        InitPoolObject(_prefab);
    }

    private void SpawnVisitor(GameObject visitorObject)
    {
        int orderInQueue = _visitorsQueue.Queue.Count;

        Visitor visitor = visitorObject.GetComponent<Visitor>();
        _visitorsQueue.Queue.Add(visitor);

        visitorObject.SetActive(true);
        visitor.Init(orderInQueue);

        visitor.Walk(_visitorsQueue.GetPositionInQueue[orderInQueue].position, _visitorStartWaiting);
    }

    private void SpawnVisitorWithGroup(GameObject visitorObject, int groupID, float wasteOfPatience)
    {
        int orderInQueue = _visitorsQueue.Queue.Count;

        Visitor visitor = visitorObject.GetComponent<Visitor>();
        _visitorsQueue.Queue.Add(visitor);
        _groups.Visitors[groupID].Add(visitor);

        visitorObject.SetActive(true);
        visitor.Init(orderInQueue, groupID, wasteOfPatience);

        visitor.Walk(_visitorsQueue.GetPositionInQueue[orderInQueue].position, _visitorStartWaiting);
    }

    private void FindFreeSlotForNewGroup(int groupMembers = 2)
    {
        if (GetFreeSlotsInQueue > 1)
        {
            if(groupMembers < 2) groupMembers = Random.Range(2, 5);

            int full = 0;
            int freeGroupSlot = _groups.FindFreeSlot();

            if(freeGroupSlot != -1)
            {
                float wasteOfPatience = Random.Range(0.5f, 3f);
                for (int i = 0; i < groupMembers; i++)
                {
                    if (TryGetObject(out GameObject vis))
                    {
                        SpawnVisitorWithGroup(vis, freeGroupSlot, wasteOfPatience);
                        if (++full >= groupMembers) break;
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }
        else
        {
            print("Нет свободных мест");
        }
    }

    private bool TrySpawnVisitor()
    {
        if (TryGetObject(out GameObject visitor))
        {
            SpawnVisitor(visitor);
            return true;
        }
        else return false;
    }

    private IEnumerator SpawnTimer()
    {
        while(true)
        {
            yield return new WaitForSeconds(_timeToCreateNewVisitor);

            if (_visitorsQueue.Queue.Count < _visitorsQueue.MAX_VISITORS_IN_QUEUE)
            {
                if (GetFreeSlotsInQueue > 1)
                {
                    int random = Random.Range(0, 3);

                    if (random == 0)
                    {
                        FindFreeSlotForNewGroup();
                    }
                    else
                    {
                        TrySpawnVisitor();
                    }
                }
                else
                {
                    TrySpawnVisitor();
                }
            }
        }
    }

    private void StartSpawnVisitors()
    {
        if(_spawnTimer != null)
        {
            StopCoroutine(_spawnTimer);
        }
        _spawnTimer = StartCoroutine(SpawnTimer());
    }

    private void StopSpawnVisitors()
    {
        if (_spawnTimer != null)
        {
            StopCoroutine(_spawnTimer);
        }
        _spawnTimer = null;
    }
}