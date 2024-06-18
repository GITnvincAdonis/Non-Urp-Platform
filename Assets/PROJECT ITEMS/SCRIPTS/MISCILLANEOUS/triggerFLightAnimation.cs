using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class triggerFLightAnimation : MonoBehaviour
{

    [SerializeField] Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(animator.GetBool("InFlight"));
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.tag);
        if(LayerMask.LayerToName(other.gameObject.layer) == "Fan") animator.SetBool("InFlight", true);
    }
    private void OnTriggerExit(Collider other)
    {
        if (LayerMask.LayerToName(other.gameObject.layer) == "Fan") animator.SetBool("InFlight", false);
        
    }
}
