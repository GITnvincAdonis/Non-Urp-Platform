using UnityEngine;
using DG.Tweening;
public class OnEnableTest : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] Transform point1;
    [SerializeField] Transform point2;
    [SerializeField] Transform point3;
    [SerializeField] CollectableSO CollectableSO;
    Vector3[] points = new Vector3[3];

    void Start()
    {
   
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    async void StartMovement(int value)
    {
        if (value == 3)
        {
            points[0] = point1.position;
            points[1] = point2.position;
            points[2] = point3.position;
            Sequence sequence = DOTween.Sequence();
            sequence.Append(transform.DOPath(points, 1, PathType.CubicBezier).SetEase(Ease.InOutSine));
            sequence.Append(transform.DOLocalMoveY(transform.position.y - 1, 4).SetEase(Ease.OutBounce));
        }
       
      
    }
    private void OnEnable()
    {
        CollectableSO.CollectableEvent.AddListener(StartMovement);    
        Debug.Log("Door enabled");
    }
 

}
