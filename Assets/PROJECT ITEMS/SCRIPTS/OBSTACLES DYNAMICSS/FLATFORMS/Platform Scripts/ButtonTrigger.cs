using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class ButtonTrigger : MonoBehaviour
{
    [SerializeField] private PlatformTImerSO timerPlatSO;
    [SerializeField] private AudioEventSO _audioSO;
    private int timer;
    Collider player;
    // Start is called before the first frame update
    void Start()
    {
        timer = timerPlatSO.TImer;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && player==null) 
        {
           
            player = other;
            timerPlatSO.RaiseTimerEvent();
            TimerMoniter();
            TogglePlatState();
        }
    }
    Task timerSound;
    async void TogglePlatState()
    {
        Debug.Log("activating Platfroms");
        await Task.Delay(timer * 1000);
        timerPlatSO.RaiseStopTimerEvent();
       
        player = null;
    }
    async Task TimerMoniter()
    {
      
        float maxTime = 6000;
        for (int i = 0; i < 15; i++) { 
            maxTime *= 0.6f;
            maxTime = Mathf.Clamp(maxTime, 20, 10000);
            await Task.Delay((int)maxTime);
           
            _audioSO.RaiseAudioEvent(AudioLibrary.instance._timePlatform,transform.position, false,false);
        }
        
        
    }
}
