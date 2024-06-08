using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroy : MonoBehaviour
{
    public static DontDestroy script;
    // Start is called before the first frame update
    void Awake()
    {
        if (script != null)
        {
            Destroy(gameObject);
            return;
        }
        script = this;
        DontDestroyOnLoad(gameObject);
    }
}
