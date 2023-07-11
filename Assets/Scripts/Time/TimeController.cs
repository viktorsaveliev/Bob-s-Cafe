using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeController : MonoBehaviour, ITimeSubject
{
    public int GetCurrentMinutes => _currentMinutes;
    public int GetCurrentHours => _currentHours;
    public int GetCurrentDay => _currentDay;
    public int GetHoursForStartDay => _hoursForStartDay;
    public int GetHoursForEndDay => _hoursForEndDay;

    private readonly float _durationForOneMinute = 0.5f;
    private readonly int _hoursForStartDay = 9;
    private readonly int _hoursForEndDay = 18;

    private int _currentMinutes;
    private int _currentHours;
    private int _currentDay;

    private bool _dayStarted;

    private Coroutine _dayTimer;
    private Action _onMinutePassed;
    private Action _onHourPassed;

    private readonly List<IStartDayObserver> _startDayObservers = new();
    private readonly List<IEndDayObserver> _endDayObservers = new();

    public void StartNewDay()
    {
        _dayStarted = true;

        _currentMinutes = 0;
        _currentHours = _hoursForStartDay;
        _currentDay++;

        if (_dayTimer != null)
        {
            StopCoroutine(_dayTimer);
        }

        _dayTimer = StartCoroutine(TimeFlow());

        foreach(IStartDayObserver observer in _startDayObservers)
        {
            if(observer == null)
            {
                print("Error: Time Init #01");
                continue;
            }
            observer.OnDayStarted();
        }
    }

    public void EndCurrentDay()
    {
        _dayStarted = false;

        if (_dayTimer != null)
        {
            StopCoroutine(_dayTimer);
        }
        _dayTimer = null;

        foreach (IEndDayObserver observer in _endDayObservers)
        {
            if (observer == null)
            {
                print("Error: Time Init #02");
                continue;
            }
            observer.OnDayEnded();
        }
    }

    private IEnumerator TimeFlow()
    {
        WaitForSeconds waitForSeconds = new(_durationForOneMinute);

        while (_dayStarted)
        {
            yield return waitForSeconds;

            if(++_currentMinutes > 59)
            {
                _currentMinutes = 0;

                if(++_currentHours == _hoursForEndDay)
                {
                    EndCurrentDay();
                }
                else
                {
                    _onHourPassed?.Invoke();
                }
            }

            _onMinutePassed?.Invoke();
        }
    }

    public string GetCurrentTime()
    {
        string minutes = _currentMinutes > 9 ? $"{_currentMinutes}" : $"0{_currentMinutes}";
        string hours = _currentHours > 9 ? $"{_currentHours}" : $"0{_currentHours}";

        string time = $"{hours}:{minutes}";
        return time;
    }

    public void SubscribeToMinutePassed(Action callback)
    {
        _onMinutePassed += callback;
    }

    public void UnsubscribeFromMinutePassed(Action callback)
    {
        _onMinutePassed -= callback;
    }

    public void SubscribeToHourPassed(Action callback)
    {
        _onHourPassed += callback;
    }

    public void UnsubscribeToHourPassed(Action callback)
    {
        _onHourPassed -= callback;
    }

    public void AddStartDayObserver(IStartDayObserver observer)
    {
        if (_startDayObservers.Contains(observer)) return;

        _startDayObservers.Add(observer);
    }

    public void RemoveStartDayObserver(IStartDayObserver observer)
    {
        if (_startDayObservers.Contains(observer))
            _startDayObservers.Remove(observer);
    }

    public void AddEndDayObserver(IEndDayObserver observer)
    {
        if (_endDayObservers.Contains(observer)) return;

        _endDayObservers.Add(observer);
    }

    public void RemoveEndDayObserver(IEndDayObserver observer)
    {
        if (_endDayObservers.Contains(observer))
            _endDayObservers.Remove(observer);
    }
}
