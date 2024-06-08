using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerInteractText : MonoBehaviour
{
    [SerializeField] UserInterfaceSO UserInterfaceSO;
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
        UserInterfaceSO.AddTextEventRaiser("Platform increases your speed.Be careful, the place is slipery");
        
        
    }
    private void OnTriggerExit(Collider other)
    {
        UserInterfaceSO.RemoveTextEventRaiser();
    }
}
