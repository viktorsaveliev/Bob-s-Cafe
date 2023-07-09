using System;
using System.Collections;
using UnityEngine;

public class Timer
{
    private readonly float _duration;
    private readonly float _tick;
    private readonly MonoBehaviour _monoBehaviour;

    private event Action OnTimerEnded;
    private event Action OnTick;

    public Timer(MonoBehaviour monoBehaviour, float duration, float tick = 1f)
    {
        _monoBehaviour = monoBehaviour;
        _duration = duration;
        _tick = tick;
    }

    private IEnumerator StartTimer()
    {
        float duration = 0;

        while(duration < _duration)
        {
            yield return new WaitForSeconds(_tick);
            OnTick?.Invoke();

            duration += _tick;
        }
        
        OnTimerEnded?.Invoke();
    }

    public void AddActionOnTimerEnd(Action method) => OnTimerEnded += method;
    public void AddActionOnSecondPassed(Action method) => OnTick += method;

    public void Start() => _monoBehaviour.StartCoroutine(StartTimer());
}
