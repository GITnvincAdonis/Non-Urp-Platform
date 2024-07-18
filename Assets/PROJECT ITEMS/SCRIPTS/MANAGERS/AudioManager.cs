using System.Threading.Tasks;
using UnityEngine;
public class AudioManager : MonoBehaviour
{

    [SerializeField] private AudioEventSO _audioEventSO;
    [SerializeField] private AudioSource _audioSource;
    private Task awaitTask;
    private void PlayAudioAtPoint(AudioClip clip, Vector3 audioPosition, bool PlayRelative, bool loopQuestion)
    {
        if(awaitTask == null)
        {
            awaitTask = StartDelay();
            AudioSource source = Instantiate(_audioSource, audioPosition, Quaternion.identity);
            Debug.Log("spawn audio");
            if(PlayRelative) source.spatialBlend = 1;
            else source.spatialBlend = 0;

            if(loopQuestion) source.loop = true;
            else source.loop = false;
            source.clip = clip;
            source.Play();
            float audioLength = source.clip.length;

            Destroy(source.gameObject,audioLength);
        }
        
    }
    private void PlayAudioAtPointAndVolume(AudioClip clip, Vector3 audioPosition, bool PlayRelative,  float volume)
    {
        if (awaitTask == null)
        {
            awaitTask = StartDelay();
            AudioSource source = Instantiate(_audioSource, audioPosition, Quaternion.identity);
            Debug.Log("spawn audio");
            if (PlayRelative) source.spatialBlend = 1;
            else source.spatialBlend = 0;

         
           
            source.volume = volume;
            source.clip = clip;
            source.Play();
            float audioLength = source.clip.length;

            Destroy(source.gameObject, audioLength);
        }

    }

    async Task StartDelay()
    {
        await Task.Delay(100);
        awaitTask =null;
    }
    private void OnEnable()
    {
        _audioEventSO.playAudioEvent.AddListener(PlayAudioAtPoint);
        _audioEventSO.playAudioAndVolumeEvent.AddListener(PlayAudioAtPointAndVolume);

    }
    private void OnDisable()
    {
        _audioEventSO.playAudioEvent.RemoveListener(PlayAudioAtPoint);
        _audioEventSO.playAudioAndVolumeEvent.RemoveListener(PlayAudioAtPointAndVolume);
    }


}
