using Cinemachine;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
[RequireComponent(typeof(Animator))]
public class PlayerCamManager : MonoBehaviour
{
    Animator animator;
    [SerializeField] CinemachineTargetGroup targetGroup;
    [SerializeField] int targetCount;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        targetCount = 0;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("yo");
        targetGroup.AddMember(other.gameObject.transform,1,0);
        targetCount++;
        if(!animator.GetBool("IsFighting")) animator.SetBool("IsFighting", true);
    }
    private void OnTriggerExit(Collider other)
    {
        targetCount --;
        targetGroup.RemoveMember(other.gameObject.transform);
        if(targetCount == 0) animator.SetBool("IsFighting", false);
    }

}
