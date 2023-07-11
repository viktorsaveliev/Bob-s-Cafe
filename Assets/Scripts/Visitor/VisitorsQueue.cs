using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class VisitorsQueue : MonoBehaviour, IVisitorActionAfterPointReached, IStartDayObserver, IEndDayObserver
{
    [SerializeField] private Transform[] _positionsInQueue;

    public int MAX_VISITORS_IN_QUEUE { get; private set; }
    public List<Visitor> Queue { get; private set; }
    public Transform[] GetPositionInQueue => _positionsInQueue;

    private bool _queueIsActive;
    private Coroutine _secondTimer;

    private void OnEnable()
    {
        EventBus.OnVisitorSitInChair += RemoveVisitorFromQueue;
    }

    private void OnDisable()
    {
        EventBus.OnVisitorSitInChair -= RemoveVisitorFromQueue;
    }

    public void Init()
    {
        MAX_VISITORS_IN_QUEUE = _positionsInQueue.Length;
        Queue = new List<Visitor>();
    }

    public void OnDayStarted()
    {
        _queueIsActive = true;
        _secondTimer = StartCoroutine(SecondTimer());
    }

    public void OnDayEnded()
    {
        ResetQueue();
        _queueIsActive = false;

        if (_secondTimer != null)
        {
            StopCoroutine(_secondTimer);
            _secondTimer = null;
        }
    }

    private IEnumerator SecondTimer()
    {
        WaitForSeconds waitForSeconds = new(1f);

        while(_queueIsActive)
        {
            yield return waitForSeconds;

            for (int i = 0; i < Queue.Count; i++)
            {
                if (Queue[i].Patience <= 0)
                {
                    VisitorGoAway(Queue[i]);
                }
            }
        }
    }

    private void VisitorGoAway(Visitor visitor, bool deleteFromQueue = true)
    {
        if(visitor.OrderInQueue != -1)
        {
            if(deleteFromQueue)
            {
                Queue.Remove(visitor);
                UpdateQueue();

                GameDataConfig difficultyLevel = GameData.Settings.CurrentDifficultyLevel;
                Money.TrySpend(difficultyLevel.PenaltyForDepartedVisitor);
            }
            EventBus.OnVisitorHasLeftQueue?.Invoke(visitor);
        }

        visitor.HUD.HideBar();
        visitor.Walk(new Vector3(13.99f, 0, 0), new VisitorDestroyer());
    }

    private void UpdateQueue()
    {
        for(int i = 0; i < Queue.Count; i++)
        {
            Queue[i].OrderInQueue = i;
            Queue[i].Walk(_positionsInQueue[i].position, this);
        }
    }

    private void RemoveVisitorFromQueue(Visitor visitor)
    {
        if (visitor == null) return;

        Queue.Remove(visitor);
        UpdateQueue();
        EventBus.OnVisitorHasLeftQueue?.Invoke(visitor);
    }

    private void ResetQueue()
    {
        foreach(Visitor visitor in Queue)
        {
            VisitorGoAway(visitor, false);
        }

        Queue.Clear();
    }

    public void OnVisitorPointReached(Visitor visitor)
    {
        visitor.Waiting();
    }
}