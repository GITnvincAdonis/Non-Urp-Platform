using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerWalkSound : MonoBehaviour
{

    [SerializeField] private AudioEventSO _audioEventSO;
    [SerializeField] private Transform _transform;
    Coroutine walkRoutine;
    [SerializeField] private Movement Movement;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float dist = Vector3.Distance(transform.position, _transform.position);
        //Debug.Log(dist);
        if (Movement.canPlayWalkAudio)
        {
            if (dist <= 0.4f && walkRoutine == null)
            {
                walkRoutine = StartCoroutine(walkSound());

            }
            else if (Movement._moveMentSpeed > 7 && dist <= 0.52f && walkRoutine == null)
            {
                walkRoutine = StartCoroutine(walkSound());
            }
        }
        
    }
    IEnumerator walkSound()
    {
        yield return new WaitForSeconds(.28f);
        _audioEventSO.RaiseAudioEventWithVolume(AudioLibrary.instance._walk, transform.position, false, 0.052f);
        walkRoutine = null;
    }

}
