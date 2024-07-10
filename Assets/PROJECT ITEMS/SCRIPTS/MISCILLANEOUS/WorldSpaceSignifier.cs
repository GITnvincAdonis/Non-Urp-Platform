using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class WorldSpaceSignifier : MonoBehaviour
{
    


    // Start is called before the first frame update
    void Start()
    {
        transform.DOLocalMoveY(3,1).SetEase(Ease.InOutCubic).SetLoops(-1,LoopType.Yoyo);
        transform.DOLocalRotate(new Vector3(transform.rotation.eulerAngles.x,360,0),4,RotateMode.FastBeyond360).SetLoops(-1).SetEase(Ease.Linear);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
