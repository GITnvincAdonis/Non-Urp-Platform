using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerInteractText : MonoBehaviour
{
    [SerializeField] UserInterfaceSO UserInterfaceSO;
    [SerializeField] string InstructionString;
    [SerializeField] Texture2D InstructionImage;
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
        UserInterfaceSO.AddTextEventRaiser(InstructionString,InstructionImage);
        
        
    }
    private void OnTriggerExit(Collider other)
    {
        UserInterfaceSO.RemoveTextEventRaiser();
    }
}
