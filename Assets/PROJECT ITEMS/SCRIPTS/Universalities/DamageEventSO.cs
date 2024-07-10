using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
[CreateAssetMenu(fileName ="DamageEventSO", menuName ="DamageEventSO")]
public class DamageEventSO : ScriptableObject
{
    // Start is called before the first frame update
    public UnityEvent damageEvent;
    public List<GameObject> targets = new();

    private void OnEnable()
    {
        if(damageEvent == null)
        {
            damageEvent = new UnityEvent();
        }
    }

    public void RaiseDamageEvent()
    {
        damageEvent?.Invoke();
    }
    public void RegisterListener(GameObject listener)
    {
        if(!targets.Contains(listener)) targets.Add(listener);
    }
}
