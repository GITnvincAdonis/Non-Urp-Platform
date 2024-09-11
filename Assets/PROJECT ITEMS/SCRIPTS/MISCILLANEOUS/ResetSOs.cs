using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetSOs : MonoBehaviour
{
    [SerializeField] CollectableSO SO1;
[SerializeField] CollectableSO SO2;
    [SerializeField] CollectableSO SO3;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnDisable()
    {
        SO1.ResetCollectables();
        SO2.ResetCollectables();
        SO3.ResetCollectables();
    }
}
