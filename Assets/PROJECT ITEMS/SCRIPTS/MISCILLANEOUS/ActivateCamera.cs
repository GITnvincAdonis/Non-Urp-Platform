using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System.Threading.Tasks;
public class ActivateCamera : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] CinemachineVirtualCamera camera; 
    [SerializeField] Movement playerScript;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnEnable()
    {
        ttask = TimedActivate();
    }
    Task ttask;
    async Task TimedActivate()
    {

        camera.Priority = 10;
        playerScript.enabled = false;
        await Task.Delay(3000);
        
        camera.Priority = 0;
        await Task.Delay(2000);
        playerScript.enabled = true;
    }
    private void OnDisable()
    {
        ttask = null;
        playerScript.enabled = true;
        camera.Priority = 0;
    }
}
