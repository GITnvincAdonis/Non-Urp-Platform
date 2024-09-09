using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerWinEvent : MonoBehaviour
{
    [SerializeField] private WinConditionSO winConSO;
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

        winConSO.RaiseWinEvent();
        
    }
}
