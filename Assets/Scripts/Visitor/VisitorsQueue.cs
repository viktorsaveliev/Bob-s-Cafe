using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class VisitorsQueue : MonoBehaviour, IVisitorActionAfterPointReached
{
    [SerializeField] private Transform[] _positionsInQueue;

    public int MAX_VISITORS_IN_QUEUE { get; private set; }
    public List<Visitor> Queue { get; private set; }
    public Transform[] GetPositionInQueue => _positionsInQueue;

    private void OnEnable()
    {
        EventBus.OnVisitorSitInChair += DeleteVisitorFromQueue;
        EventBus.OnCurrentDayEnded += ResetQueue;
    }

    private void OnDisable()
    {
        EventBus.OnVisitorSitInChair -= DeleteVisitorFromQueue;
        EventBus.OnCurrentDayEnded -= ResetQueue;
    }

    public void Init()
    {
        MAX_VISITORS_IN_QUEUE = _positionsInQueue.Length;
        Queue = new List<Visitor>();

        StartCoroutine(SecondTimer());
    }

    private IEnumerator SecondTimer()
    {
        yield return new WaitForSeconds(1f);

        for (int i = 0; i < Queue.Count; i++)
        {
            if (Queue[i].Patience <= 0)
            {
                VisitorGoAway(Queue[i]);
            }
        }

        StartCoroutine(SecondTimer());
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
                Money.Spend(difficultyLevel.PenaltyForDepartedVisitor);
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

    private void DeleteVisitorFromQueue(Visitor visitor)
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