using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CAMERASpeedLinker : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] Movement playerMovement;
    [SerializeField] CinemachineVirtualCamera cam;
    void Start()
    {

        cam.m_Lens.FieldOfView = 30;
    }
    float yVelocity = 0.0f;
    // Update is called once per frame
    void Update()
    {
        
        cam.m_Lens.FieldOfView = Mathf.SmoothDamp(cam.m_Lens.FieldOfView, 30 + playerMovement._moveMentSpeed, ref yVelocity , 5, 15f);
            
    }
}
