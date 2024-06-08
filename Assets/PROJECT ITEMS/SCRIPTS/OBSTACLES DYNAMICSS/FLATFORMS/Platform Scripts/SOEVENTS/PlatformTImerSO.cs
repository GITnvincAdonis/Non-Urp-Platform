using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName ="PlatformTImerSO", menuName = "PlatformTImerSO")]
public class PlatformTImerSO : ScriptableObject
{
    public UnityEvent StartTimerEvent;
    public UnityEvent StopTimerEvent;
    public int TImer;
    public List<GameObject> listeners = new ();

    private void OnEnable()
    {
        if (StartTimerEvent != null) StartTimerEvent = new UnityEvent();
        if (StopTimerEvent != null) StopTimerEvent = new UnityEvent();
    }
    public void RegisterListener(GameObject listener)
    {
        if (!listeners.Contains(listener))
        {
            listeners.Add(listener);
        }
    }
    public void UnregisterListener(GameObject listener) 
    {
        if (listeners.Contains(listener))
        {
            listeners.Remove(listener);
        }
    }

    public void RaiseTimerEvent()
    {
        StartTimerEvent?.Invoke();
    }
    public void RaiseStopTimerEvent()
    {
        StopTimerEvent?.Invoke();
    }

}
