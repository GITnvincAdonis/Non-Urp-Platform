using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Threading.Tasks;
using Unity.Loading;


public class TakeDamage : MonoBehaviour,IDamagable
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
    [SerializeField] private HealthSOScript dronehealth;

    [SerializeField] private Movement attachedScript;
    [SerializeField] private CharacterController characterController;

    private void Awake()
    {
        if (damagableType == DamageableType.Player)
        {
            attachedScript = GetComponent<Movement>();
            characterController = GetComponent<CharacterController>();
        }
    }
    public void TakeHurt()
    {
        if (damagableType == DamageableType.Player)
        {
            healthRef.ReduceHealth(15);
            float health = healthRef.healthLevel;
            if (health <= 0 && damagableType == DamageableType.Player) { 
                healthRef.ResetHealth();
                winSO.RaiseLoseEvent(); 
                    
            }
            

            
            Debug.Log(health);
            uiObject.ChangeHealthBar((int)health);
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
