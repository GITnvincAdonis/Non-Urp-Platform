using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "WinConSO", menuName = "WinConSO")]
public class WinConditionSO : ScriptableObject
{
    public UnityEvent WinEvent;
    public UnityEvent LoseEvent;
    private void OnEnable()
    {
        if (WinEvent != null)
        {
            WinEvent = new UnityEvent();
        }
        if ( LoseEvent != null)
        {
            LoseEvent = new UnityEvent();   
        }
    }

    public void RaiseWinEvent()
    {
        Debug.Log("won");
        WinEvent?.Invoke();
    }
    public void RaiseLoseEvent()
    {
        Debug.Log("lost");
        LoseEvent?.Invoke();
    }
}
