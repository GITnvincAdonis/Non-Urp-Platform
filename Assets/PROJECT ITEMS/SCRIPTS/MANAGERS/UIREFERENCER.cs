using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIREFERENCER : MonoBehaviour
{


    public HealthSOScript healthScriptReference;





    private int HealthValue;

    // Start is called before the first frame update
    void Start()
    {
        AcceptHealth(healthScriptReference.healthLevel);
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    private void OnEnable()
    {
        healthScriptReference.healthEvent.AddListener(AcceptHealth);
        healthScriptReference.RegisterEvent(gameObject);
    }
    private void OnDisable()
    {
        healthScriptReference.healthEvent.RemoveListener(AcceptHealth);
        healthScriptReference.UnregisterEvent(gameObject);
    }


    void AcceptHealth(int health)
    {
        HealthValue = health;
    }
}
