using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class PlayOnAwake : MonoBehaviour
{
    [SerializeField] private AudioEventSO AudioEventSO;
    // Start is called before the first frame update
    void Start()
    {
        Replay();
    }
    async void Replay()
    {
        while (true)
        {
            AudioEventSO.RaiseAudioEvent(AudioLibrary.instance._blowingFan, transform.position, true, true);
            await Task.Delay((int)AudioLibrary.instance._blowingFan.length * 1000);
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
