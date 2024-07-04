using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName ="HealthContainer", menuName ="HealthContainer")]
public class HealthSOScript : ScriptableObject
{
    public List<GameObject> Listeners = new();
    public UnityEvent<int> healthEvent;
    public int healthLevel = 100;

    private void OnEnable(){
        healthLevel = 100;
        if (healthEvent != null) healthEvent = new UnityEvent<int>();
    }
    public void RegisterEvent(GameObject script){
        if (!Listeners.Contains(script)) Listeners.Add(script);
    }
    public void UnregisterEvent(GameObject script){
        if(Listeners.Contains(script)) Listeners.Remove(script);         
    }


    public void ReduceHealth(int healthDecrease) { 
        healthLevel -= healthDecrease;
        healthEvent.Invoke(healthLevel);
    }
    public void IncreaseHealth(int healthIncrease)
    {
        healthLevel += healthIncrease;
        healthEvent.Invoke(healthLevel);
    }
    public void ResetHealth()
    {
        healthLevel = 100;
        healthEvent.Invoke(healthLevel);
    }



}
