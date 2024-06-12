using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerInteractText : MonoBehaviour
{
    [SerializeField] UserInterfaceSO UserInterfaceSO;
    [SerializeField] string InstructionString;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        UserInterfaceSO.AddTextEventRaiser(InstructionString);
        
        
    }
    private void OnTriggerExit(Collider other)
    {
        UserInterfaceSO.RemoveTextEventRaiser();
    }
}
