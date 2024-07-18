using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioLibrary : MonoBehaviour
{
    // Start is called before the first frame update
    public static AudioLibrary instance;

    [SerializeField] public AudioClip _LandingSound;
    [SerializeField] public AudioClip _launchSound;
    [SerializeField] public  AudioClip _playSound;
    [SerializeField] public AudioClip _rocketWhistle;
    [SerializeField] public AudioClip _rocketExplosion;
    [SerializeField] public AudioClip _hurt;
    [SerializeField] public AudioClip _cannonFire;
    [SerializeField] public AudioClip _blowingFan;
    [SerializeField] public AudioClip _platformCancel;
    [SerializeField] public AudioClip _cannonMove;
    [SerializeField] public AudioClip _timePlatform;
    [SerializeField] public AudioClip _walk;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }
  
}
