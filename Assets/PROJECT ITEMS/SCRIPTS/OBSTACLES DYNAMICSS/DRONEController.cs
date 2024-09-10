using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Data.Common;
using System.Threading.Tasks;
using Unity.VisualScripting;
using TMPro;
[RequireComponent(typeof(CharacterController))]
public class DRONEController : MonoBehaviour
{
    public enum DroneState
    {
        idle,
        RestReturn,
        Attack,
        Recoil
    }
    [SerializeField] RocketInstructions rockets;
    [SerializeField] private int _MinShootInterval;
    [SerializeField] private int _MaxShootInterval;
    [SerializeField] private float _slerpSpeed;
    [SerializeField] private int _burstCount;
    [SerializeField] private int _playerOffset;
    [SerializeField] private Transform _playerPos;
    [SerializeField] private ScreenShakeSO screenShakeSO;
    [SerializeField] private ParticleSystem _explosionParticles;
    [SerializeField] private GameObject _droneMesh;
    [SerializeField] private AudioEventSO _audioEvent;
    public DroneState _currentState;
    CharacterController _droneController;
    Vector3 _playerDestination;

    Task _updateDroneposition;
    Task _updateDroneYPos;
    Task _shootUpdate;

    Vector3 _restPosition;


    // Start is called before the first frame update
    private void Awake()
    {
        
        ChangeState(DroneState.idle);
        
        _droneController = GetComponent<CharacterController>();
    }
    void Start()
    {
        _restPosition = transform.position;

    }





    void AttackStateConditions()
    {
        if(_playerDestination != Vector3.zero)
        {
            Quaternion _rotation = Quaternion.LookRotation(_playerDestination - transform.position);
            Quaternion _currentRotation = transform.rotation;
            transform.rotation = Quaternion.Slerp(_currentRotation,_rotation, _slerpSpeed);

        }
        if(_updateDroneposition == null )
        {
            _updateDroneposition = UpdateDrone(); 

        }
        if (_updateDroneYPos == null)
        {
            _updateDroneYPos = UpdateDroneYpos();
        }
        if(_shootUpdate == null)
        {
            _shootUpdate = Shoot();
        }

    }
    async Task Shoot()
    {
        
        for (int i = 0; i < _burstCount; i++)
        {
            GameObject instantiatedRocket = Instantiate(rockets.gameObject, transform.position + transform.forward, transform.rotation);
            instantiatedRocket.GetComponent<RocketInstructions>().GatherPlayerPosition(transform.position + (transform.forward) * 2);
            await Task.Delay(400);
        }
        int seed = Random.Range(_MinShootInterval, _MaxShootInterval);
        await Task.Delay(seed * 1000);
        _shootUpdate = null;
    }
    async Task UpdateDrone()
    {
        await Task.Delay(100);
        Vector3 playerPos;
        playerPos.x = _playerDestination.x;
        playerPos.z = _playerDestination.z;
        transform.DOMoveX(playerPos.x,3).SetEase(Ease.InOutQuad);
        transform.DOMoveZ(playerPos.z, 3).SetEase(Ease.InOutQuad).OnComplete(() =>
        {
            _updateDroneposition = null;
        });
        
    }
    async Task UpdateDroneYpos()
    {
        await Task.Delay(10);
        float destination = 0;
       
        destination = _playerOffset + (_playerDestination.y* 0.5f)+ Mathf.Sin(Time.time + 10);
        transform.DOMoveY(destination, 5f).SetEase(Ease.InOutQuad).OnComplete(() =>
        {
            _updateDroneYPos = null;
        });
        
    }

    void ChangeState(DroneState state) {
        
        _currentState = state;
       
    }
    void RestReturnLogic()
    {

       
        _playerDestination = _restPosition;
        if (_playerDestination != Vector3.zero)
        {
            Quaternion _rotation = Quaternion.LookRotation(_playerDestination - transform.position);
            Quaternion _currentRotation = transform.rotation;
            transform.rotation = Quaternion.Slerp(_currentRotation, _rotation, _slerpSpeed);

        }
        if (_updateDroneposition == null)
        {
            _updateDroneposition = UpdateDrone();

        }
       
        if (Vector3.Distance(transform.position,_restPosition) <= 6f) {
            ChangeState(DroneState.idle);
        }
        

    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (_currentState == DroneState.RestReturn)
        {
            RestReturnLogic();
            Vector3 destination = Vector3.zero;
            destination.y = Mathf.Sin(Time.time + 10);
        }
        if (_currentState == DroneState.Attack)
        {
            AttackStateConditions();
            Vector3 destination = Vector3.zero;
            destination.y = Mathf.Sin(Time.time + 10);
           
        }
        if (_currentState == DroneState.Recoil)
        {
            Vector3 playerPos = _playerPos.position;

            Vector3 direction = transform.position - playerPos;
            if(recoilTask == null)
            {
                recoilTask = RecoilAction(direction.normalized * .1f);
            }
            
            Debug.Log("recoiling");
        }
        if (_currentState == DroneState.RestReturn) Debug.Log(_playerDestination);
        if(Vector3.Distance(transform.position,_playerPos.position) > 35 && (_currentState == DroneState.Attack))
        {
                ChangeState(DroneState.RestReturn);

        } if(Vector3.Distance(transform.position,_playerPos.position) < 35 && (_currentState != DroneState.Attack && _currentState != DroneState.Recoil))
        {   
                ChangeState(DroneState.Attack);
        }


    }
    Task recoilTask = null;
    async Task RecoilAction(Vector3 direction)
    {
        screenShakeSO.TriggerScreenShake();
        transform.DOMove(direction, 7).SetEase(Ease.Linear);
        
        await Task.Delay(1400);
        _explosionParticles.Play();
        _droneMesh.SetActive(false);
        _audioEvent.RaiseAudioEventWithVolume(AudioLibrary.instance._rocketExplosion, transform.position, false, .8f);
        Destroy(gameObject, .5f);


    }

    private void OnTriggerStay(Collider other)
    {
        _playerDestination = _playerPos.position;
    }
 



}
