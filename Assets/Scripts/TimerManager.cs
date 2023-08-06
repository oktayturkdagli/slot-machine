using System;
using System.Collections;
using UnityEngine;

public class TimerManager : MonoBehaviour
{
    private const float TimeSensitivity = 0.01f;
    
    public void CreateTimer(float targetTime, Action onTimeOutActions)
    {
        var timer = new Timer(targetTime, onTimeOutActions);
        var timerCoroutine = TimerCoroutine(timer);
        StartCoroutine(timerCoroutine);
    }
    
    private IEnumerator TimerCoroutine(Timer timer)
    {
        while (timer.CurrentTime < timer.TargetTime)
        {
            yield return new WaitForSecondsRealtime(TimeSensitivity);
            timer.CurrentTime += TimeSensitivity;
        }
        
        timer.OnTimeOutEvent?.Invoke();
        timer.OnTimeOutEvent = null;
    }
}

public class Timer
{
    public float CurrentTime;
    public readonly float TargetTime;
    public Action OnTimeOutEvent;
    
    public Timer(float targetTime, Action onTimeOutActions)
    {
        CurrentTime = 0;
        TargetTime = targetTime;
        OnTimeOutEvent = onTimeOutActions;
    }
}