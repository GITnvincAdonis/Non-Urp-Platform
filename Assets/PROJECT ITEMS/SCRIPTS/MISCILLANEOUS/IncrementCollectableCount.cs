using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncrementCollectableCount : MonoBehaviour
{

    // Start is called before the first frame update
    [SerializeField] private CollectableSO CollectableSO;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        CollectableSO.IncrementCollectable();
    }
   
}
