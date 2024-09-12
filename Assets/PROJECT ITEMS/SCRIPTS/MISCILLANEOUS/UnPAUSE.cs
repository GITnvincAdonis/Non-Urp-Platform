using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnPAUSE : MonoBehaviour
{
    // Start is called before the first frame update

    void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnEnable()
    {
        Debug.Log(Time.timeScale);
    }

}
