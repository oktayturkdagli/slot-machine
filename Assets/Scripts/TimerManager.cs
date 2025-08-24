using System;
using System.Collections;
using UnityEngine;

public class TimerManager : MonoBehaviour
{
    private const float TimeSensitivity = 0.01f;
    
    // Creates and initializes a new timer object
    public void CreateTimer(float targetTime, Action onTimeOutActions)
    {
        var timer = new Timer(targetTime, onTimeOutActions);
        var timerCoroutine = TimerCoroutine(timer);
        StartCoroutine(timerCoroutine);
    }
    
    // Creates a stopwatch that counts up to the specified time, the OnTimeOutEvent invokes when the time expires
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

public struct Timer
{
    public float CurrentTime { get; set; }
    public float TargetTime { get; set; }
    public Action OnTimeOutEvent { get; set; }

    public Timer(float targetTime, Action onTimeOutActions)
    {
        CurrentTime = 0;
        TargetTime = targetTime;
        OnTimeOutEvent = onTimeOutActions;
    }
}