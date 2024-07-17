using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
[RequireComponent(typeof(CharacterController))]
public class RocketInstructions : MonoBehaviour
{
    [SerializeField] private AudioEventSO _audioSO;
    [SerializeField] GameObject _particles;
    CharacterController controller;
    float Speed;
    Vector3 destination;
    Vector3 directions;
    // Start is called before the first frame update
    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        Speed = 20;
        Destroy(gameObject, 5);
        _audioSO.RaiseAudioEvent(AudioLibrary.instance._rocketWhistle, transform.position,true, false);
        
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (destination != null)
        {
            
            controller.Move(directions * Time.fixedDeltaTime* 25);
        }
    }
    public void GatherPlayerPosition(Vector3 pos)
    {
        destination = pos;
        directions = (destination - transform.position).normalized;
    }

    private void OnDestroy()
    {
        Collider[] collider = Physics.OverlapSphere(transform.position, 3);

        foreach (Collider collider2 in collider)
        {
            if (collider2.gameObject.GetComponent<IDamagable>() != null && !collider2.CompareTag("Drone"))
            {
                Debug.Log("HITTTT");
                collider2.GetComponent<IDamagable>().TakeHurt();
                break;
            }
            
            Debug.Log(collider2.name);

        }
        GameObject Localparticle = Instantiate(_particles, transform.position, Quaternion.identity);
        Localparticle.GetComponent<ParticleSystem>().Play();
        _audioSO.RaiseAudioEvent(AudioLibrary.instance._rocketExplosion, transform.position, true, false);

    }
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {

        
        

        if (hit.collider.GetComponent<IDamagable>() != null) {
            Debug.Log("Hit player");
            
        }
        if (!hit.collider.CompareTag("Drone")) Destroy(gameObject);
        
    }
}
