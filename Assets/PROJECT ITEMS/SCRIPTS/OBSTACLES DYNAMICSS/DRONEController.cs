using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Data.Common;
using System.Threading.Tasks;
using Unity.VisualScripting;
[RequireComponent(typeof(CharacterController))]
public class DRONEController : MonoBehaviour
{
    public enum DroneState
    {
        idle,
        Attack
    }
    [SerializeField] RocketInstructions rockets;
    [SerializeField] private int _MinShootInterval;
    [SerializeField] private int _MaxShootInterval;
    [SerializeField] private float _slerpSpeed;
    [SerializeField] private int _burstCount;
    [SerializeField] private int _playerOffset;

    DroneState _currentState;
    CharacterController _droneController;
    Vector3 _playerDestination;

    Task _updateDroneposition;
    Task _updateDroneYPos;
    Task _shootUpdate;
    // Start is called before the first frame update
    private void Awake()
    {
        _currentState = DroneState.idle;
        _droneController = GetComponent<CharacterController>();
    }
    void Start()
    {
        
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
            await Task.Delay(20);
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
        //destination.x = _playerDestination.x - transform.position.x;
        destination = _playerOffset +  _playerDestination.y + Mathf.Sin(Time.time + 10);
        transform.DOMoveY(destination, 1f).SetEase(Ease.InOutQuad).OnComplete(() =>
        {
            _updateDroneYPos = null;
        });
    }
    void IdleDroneState()
    {

    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (_currentState == DroneState.idle)
        {
            IdleDroneState();   
        }
        if (_currentState == DroneState.Attack)
        {
            AttackStateConditions();
            Vector3 destination = Vector3.zero;
            //destination.x = _playerDestination.x - transform.position.x;
            destination.y = Mathf.Sin(Time.time + 10);
            //destination.z = _playerDestination.z - transform.position.z;

            //_droneController.Move(destination.normalized * Time.fixedDeltaTime);
        }
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (_currentState == DroneState.idle) {
            _currentState = DroneState.Attack;
           
        }
        Debug.Log("we in");
    }
    private void OnTriggerStay(Collider other)
    {
        _playerDestination = other.transform.position;
    }
    private void OnTriggerExit(Collider other)
    {
        if (_currentState == DroneState.Attack){
            _currentState = DroneState.idle;
            _playerDestination = Vector3.zero;
        }
        Debug.Log("we out");
    }
}
