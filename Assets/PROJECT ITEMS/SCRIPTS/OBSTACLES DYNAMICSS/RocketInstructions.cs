using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
[RequireComponent(typeof(CharacterController))]
public class RocketInstructions : MonoBehaviour
{
    CharacterController controller;
    float Speed;
    Vector3 destination;
    Vector3 directions;
    // Start is called before the first frame update
    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        Speed = 20;
        
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (destination != null)
        {
            
            controller.Move(directions * Time.fixedDeltaTime* 10);
        }
    }
    public void GatherPlayerPosition(Vector3 pos)
    {
        destination = pos;
        directions = (destination - transform.position).normalized;
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Destroy(gameObject);
    }
}
