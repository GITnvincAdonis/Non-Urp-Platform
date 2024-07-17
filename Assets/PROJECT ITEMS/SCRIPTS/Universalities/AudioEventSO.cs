
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "AudioSO",  menuName = "AudioSO")]
public class AudioEventSO : ScriptableObject
{
    public UnityEvent<AudioClip, Vector3, bool, bool> playAudioEvent;

    public AudioClip playAudioClip;
    public Vector3 position;
    public bool PlayInWorld;
    public bool Loop;
    // Start is called before the first frame update
    private void OnEnable()
    {
        if(playAudioEvent == null)
        {
            playAudioEvent = new UnityEvent<AudioClip, Vector3, bool, bool>();   
        }
    }

    public void RaiseAudioEvent(AudioClip clipToPlay, Vector3 postionToSpawn , bool playRelative, bool loopClip)
    {
        playAudioClip = clipToPlay;
        position = postionToSpawn;
        PlayInWorld = playRelative;
        Loop = loopClip;
        playAudioEvent?.Invoke( clipToPlay, position, PlayInWorld, Loop);
    }
}
