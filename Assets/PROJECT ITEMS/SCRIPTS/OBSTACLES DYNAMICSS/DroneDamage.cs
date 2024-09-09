using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

public class DroneDamage : MonoBehaviour
{
    [SerializeField] DRONEController DRONEController;
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
        Debug.Log(other.tag);
        Vector3 playerPos = other.transform.position;

        Vector3 direction  = transform.position - playerPos;
        DRONEController._currentState = DRONEController.DroneState.Recoil;

    }
    async void RecoilAction(Vector3 direction)
    {
       
        float time = 0;
        while(time < 3)
        {
            
            
        }
        //DRONEController.gameObject.SetActive(false);
    }
}
