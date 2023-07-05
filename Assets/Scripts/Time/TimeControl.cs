using System;
using System.Collections;
using UnityEngine;

public class TimeControl : MonoBehaviour
{
    public int GetCurrentMinutes => _currentMinutes;
    public int GetCurrentHours => _currentHours;
    public int GetCurrendDay => _currentDay;
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
        EventBus.OnNewDayStarted?.Invoke();
    }

    public void EndCurrentDay()
    {
        _dayStarted = false;

        if (_dayTimer != null)
        {
            StopCoroutine(_dayTimer);
        }
        _dayTimer = null;

        EventBus.OnCurrentDayEnded?.Invoke();
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
}
