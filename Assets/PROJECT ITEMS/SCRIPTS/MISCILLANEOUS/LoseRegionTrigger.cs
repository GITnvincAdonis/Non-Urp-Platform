using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoseRegionTrigger : MonoBehaviour
{
    // Start is called before the first frame update
    public WinConditionSO winConSO;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {

        winConSO.RaiseLoseEvent();
    }
}
