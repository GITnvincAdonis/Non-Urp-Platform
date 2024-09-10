using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class Collectables : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private float RotationSpeed, OffsetSpeed, DisplacementAmount;
    public UserInterfaceSO UIreferences;
    public Texture2D image1,image2,image3;

    [SerializeField] private int CollectableNumber;

    bool isMoving;

    DG.Tweening.Tween _moveTween;
    DG.Tweening.Tween _rotateTween;
    void Start()
    {

        float yPos = transform.position.y;
        _moveTween = transform.DOLocalMoveY( yPos + DisplacementAmount,OffsetSpeed).SetEase(Ease.InOutCubic).SetLoops(-1,LoopType.Yoyo);
        _rotateTween = transform.DORotate(new Vector3(0,360,0),RotationSpeed,RotateMode.FastBeyond360).SetEase(Ease.Linear).SetLoops(-1);
        
    }

    // Update is called once per frame
    void Update()
    {
        
        
            
            
        
      
    }
    
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name);
        if(CollectableNumber ==1) UIreferences.ChangeContOneEventRaiser(image1);  
        if(CollectableNumber ==2) UIreferences.ChangeContTwoEventRaiser(image2);
        if(CollectableNumber ==3) UIreferences.ChangeContThreeEventRaiser(image3);
        _moveTween.Kill();
        _rotateTween.Kill();
        Destroy(gameObject);
    }
}
