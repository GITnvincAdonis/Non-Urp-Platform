using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerEndCannon : MonoBehaviour
{
    [SerializeField] private GameObject cannon;
    [SerializeField] private CollectableSO collectableSOObject;
    // Start is called before the first frame update
    void Start()
    {
            cannon.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void ActivateCannon()
    {
        cannon.SetActive(true);
    }
    private void OnEnable()
    {
        collectableSOObject.CollectableEvent.AddListener(ActivateCannon);
    }

}
