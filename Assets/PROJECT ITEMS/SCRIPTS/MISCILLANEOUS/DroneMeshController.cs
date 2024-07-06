using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneMeshController : MonoBehaviour
{
    [SerializeField] private Transform targetInfo;
    [SerializeField] private Transform launchers;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       
        transform.position = targetInfo.position;
        Vector3 rawRotation = transform.rotation.eulerAngles;
        rawRotation.z = Mathf.Clamp(rawRotation.z, 10, 30);

        float tempRot = rawRotation.z;
       
        transform.rotation = Quaternion.Euler(new Vector3(rawRotation.x, rawRotation.y, tempRot));
        //transform.localRotation = targetInfo.localRotation;


    }
}
