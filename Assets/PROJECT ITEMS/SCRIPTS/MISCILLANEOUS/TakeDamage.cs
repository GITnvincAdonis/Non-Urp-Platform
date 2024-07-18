using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Threading.Tasks;
using Unity.Loading;
using Cinemachine;
using TreeEditor;


public class TakeDamage : MonoBehaviour, IDamagable
{
    public enum DamageableType
    {
        Player,
        Drone
    }





    public DamageableType damagableType;
    [SerializeField] private HealthSOScript healthRef;
    [SerializeField] private UserInterfaceSO uiObject;
    [SerializeField] private WinConditionSO winSO;
    [SerializeField] private AudioEventSO audioEvent;

    [SerializeField] private DamageEventSO damageEvent;
    CinemachineImpulseSource CinemachineImpulseSource;

    private void Awake()
    {
        CinemachineImpulseSource = GetComponent<CinemachineImpulseSource>();
       
    }
    public void TakeHurt()
    {
        audioEvent.RaiseAudioEvent(AudioLibrary.instance._hurt, transform.position, false, false);
        if (damagableType == DamageableType.Player)
        {
            healthRef.ReduceHealth(35);
            float health = healthRef.healthLevel;
            if (health <= 0 && damagableType == DamageableType.Player) { 
                healthRef.ResetHealth();
                winSO.RaiseLoseEvent(); 
                    
            }
            

            
            Debug.Log(health);
            uiObject.ChangeHealthBar((int)health);
            CinemachineImpulseSource.GenerateImpulseWithForce(3);
            damageEvent.RaiseDamageEvent();
            
        }
    }
    private void OnEnable()
    {
        if (damagableType == DamageableType.Player)
        {
            healthRef.healthEvent.AddListener(handleKnockBackLogic);
            healthRef.RegisterEvent(gameObject);
        }
        
    }
    private void OnDisable()
    {
        if (damagableType == DamageableType.Player)
        {
            healthRef.healthEvent.AddListener(handleKnockBackLogic);
            healthRef.UnregisterEvent(gameObject);
        }
        
    }

    void handleKnockBackLogic(int DiscardedVal)
    {
        //disable character controller and script
        //tween objects with convincing bounce
    }


}
