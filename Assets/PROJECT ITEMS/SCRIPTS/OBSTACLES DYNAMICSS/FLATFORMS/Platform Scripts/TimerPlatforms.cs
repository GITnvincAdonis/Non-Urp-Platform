
using System.Collections.Generic;
using UnityEngine;

public class TimerPlatforms : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] List<MeshCollider> platforms = new();
    [SerializeField] private PlatformTImerSO timerPlatformSO;
    private void Awake()
    {
        foreach (MeshCollider platform in platforms)
        {
            platform.GetComponent<MeshRenderer>().enabled = false;
        }
    }
    
   
    // Update is called once per frame
 
    private void OnEnable()
    {
        timerPlatformSO.StartTimerEvent.AddListener(PlatformStartEvent);
        timerPlatformSO.StopTimerEvent.AddListener(PlatformEndEvent);
        timerPlatformSO.RegisterListener(gameObject);
    }
    private void OnDisable()
    {
        timerPlatformSO.StartTimerEvent.RemoveListener(PlatformStartEvent);
        timerPlatformSO.StopTimerEvent.RemoveListener(PlatformEndEvent);
        timerPlatformSO.UnregisterListener(gameObject);
    }

    private void PlatformStartEvent()
    {
        Debug.Log("yo");
        foreach (MeshCollider platform in platforms)
        {
            MeshRenderer appearance = platform.GetComponent<MeshRenderer>();
            if (appearance != null) appearance.enabled = true;
            platform.isTrigger = false;
        }
    }
    private void PlatformEndEvent()
    {
        foreach (MeshCollider platform in platforms)
        {
            MeshRenderer appearance = platform.GetComponent<MeshRenderer>();
            if (appearance != null) appearance.enabled = false;
            platform.isTrigger = true;

        }
    }
}
