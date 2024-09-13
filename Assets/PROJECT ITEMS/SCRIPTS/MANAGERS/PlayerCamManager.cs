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
    [SerializeField] List<GameObject> targets = new List<GameObject>();
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
        
        if (!targets.Contains(other.gameObject))
        {
            targets.Add(other.gameObject);
            targetCount++;
            targetGroup.AddMember(other.gameObject.transform, 1, 0);
        }
        

        if(!animator.GetBool("IsFighting")) animator.SetBool("IsFighting", true);
    }
    private void OnTriggerExit(Collider other)
    {
        if (targets.Contains(other.gameObject))
        {
            targets.Remove(other.gameObject);
            targetCount--;
            targetGroup.RemoveMember(other.gameObject.transform);
        }
       
        
        if(targetCount <= 0) animator.SetBool("IsFighting", false);
    }
    public void RemoveElementFromList(GameObject el)
    {
        if(targetGroup.FindMember(el.transform) != -1)
        {
            targetCount--;
            targets.Remove(el.gameObject);  
            targetGroup.RemoveMember(el.transform);
            
        }
        if (targetCount == 0) animator.SetBool("IsFighting", false);
    }

}
