using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "CollectableSO", fileName = "CollectableSO")]


public class CollectableSO : ScriptableObject
{
    // Start is called before the first frame update
   
    public int collectableCount = 0;
    public UnityEvent<int> CollectableEvent = new();
    private void OnEnable()
    {
       collectableCount = 0;
        if (CollectableEvent == null)
        {
            CollectableEvent= new ();
        }
    }

    public void IncrementCollectable()
    {
        collectableCount++;
        CollectableEvent.Invoke(collectableCount);
        
    }

 
    public void ResetCollectables()
    {
        collectableCount = 0;
        CollectableEvent.Invoke(collectableCount);
    }
}
