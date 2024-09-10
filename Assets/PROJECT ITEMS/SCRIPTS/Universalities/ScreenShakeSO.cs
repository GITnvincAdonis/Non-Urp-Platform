using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;
[CreateAssetMenu(fileName ="ScreenShakeSO", menuName ="ScreenShakeSO")]
public class ScreenShakeSO : ScriptableObject
{
    public UnityEvent shakeEvent;
    private void OnEnable()
    {
        if (shakeEvent != null) {
            shakeEvent = new UnityEvent();
        }
    }
    public void TriggerScreenShake()
    {
        shakeEvent?.Invoke();
    }
}
