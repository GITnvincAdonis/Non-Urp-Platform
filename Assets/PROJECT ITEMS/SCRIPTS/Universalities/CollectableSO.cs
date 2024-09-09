using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "CollectableSO", fileName = "CollectableSO")]


public class CollectableSO : ScriptableObject
{
    // Start is called before the first frame update
   
    public int collectableCount = 0;
    public UnityEvent CollectableEvent = new();
    private void OnEnable()
    {
        collectableCount = 0;
        if (CollectableEvent == null)
        {
            CollectableEvent= new UnityEvent();
        }
    }

    public void IncrementCollectable()
    {
        collectableCount++;
        if (collectableCount > 2) CompletedEvent();
    }

    public void CompletedEvent() { 
        CollectableEvent.Invoke();
    }
}
