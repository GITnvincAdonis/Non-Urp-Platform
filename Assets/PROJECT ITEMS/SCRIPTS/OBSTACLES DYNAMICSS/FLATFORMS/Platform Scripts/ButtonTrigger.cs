using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class ButtonTrigger : MonoBehaviour
{
    [SerializeField] private PlatformTImerSO timerPlatSO;
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
            TogglePlatState();
        }
    }
    async void TogglePlatState()
    {
        Debug.Log("activating Platfroms");
        await Task.Delay(timer * 1000);
        timerPlatSO.RaiseStopTimerEvent();
        player = null;
    }

}
