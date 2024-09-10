using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class ShakeManager : MonoBehaviour
{
    CinemachineImpulseSource CinemachineImpulseSource;
    [SerializeField] private ScreenShakeSO ScreenShakeSO;
    // Start is called before the first frame update
    void Start()
    {
        CinemachineImpulseSource = GetComponent<CinemachineImpulseSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnEnable()
    {
        ScreenShakeSO.shakeEvent.AddListener(TriggerShake);
    }
    private void TriggerShake()
    {
        CinemachineImpulseSource.GenerateImpulseWithForce(3);
    }
}
