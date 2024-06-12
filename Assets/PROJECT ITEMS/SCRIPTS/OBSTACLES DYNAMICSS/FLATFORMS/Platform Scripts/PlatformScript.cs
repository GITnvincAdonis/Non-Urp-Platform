using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Threading.Tasks;
using Unity.Loading;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine.Rendering;



public class PlatformScript : MonoBehaviour
{
    public enum PlatformBehaviour
    {
        rotatingX,
        rotatingY,
        rotatingZ,
        LineMove,
        oneWay,
        FallThrough,
        Bounce,
        Swinging,
        Speeding,
        UpWardBlowers
    }
    public PlatformBehaviour BehaviourType;
    [SerializeField] private float RotationDuration;


    [SerializeField] private Vector3 Displacement;
    [SerializeField] private float DisplacementDuration;
    [SerializeField] private float BounceForce;

    [SerializeField] private Collider collider;
    [SerializeField] private Collider Trigger;

    [SerializeField] private Rigidbody rb;
    [SerializeField] private int fallTimer;

    [SerializeField] private Transform SwingPoint1;
    [SerializeField] private Transform SwingPoint2;   
    [SerializeField] private Transform SwingPoint3;
    [SerializeField] private float SwingSpeed;

    [SerializeField] private float SpeedIncrease;
    [SerializeField] private float upwardsForce;
    [SerializeField] private Transform DirectionSource;

    private Vector3 initialPos;

    List<Tween> tweens = new ();
    // Start is called before the first frame update
    private void Awake()
    {
        initialPos = transform.position;    
        if (BehaviourType == PlatformBehaviour.oneWay)  collider.enabled =false;
         
    }
    void Start()
    {
        RotationLogic();
        LinearMovementLogic();
        OneWayLogic();
        SwingLogic();
        FallLogic();
    }
    void RotationLogic()
    {
        if (BehaviourType == PlatformBehaviour.rotatingX)
        {
            Vector3 rotationDestination = Vector3.zero;
            rotationDestination.x = 360f;

            tweens.Add(transform.DORotate(rotationDestination, RotationDuration, RotateMode.FastBeyond360).SetLoops(-1).SetEase(Ease.Linear));

        }
        if (BehaviourType == PlatformBehaviour.rotatingY)
        {
            Vector3 rotationDestination = Vector3.zero;
            rotationDestination.y = 360f;

            tweens.Add(transform.DORotate(rotationDestination, RotationDuration, RotateMode.FastBeyond360).SetLoops(-1).SetEase(Ease.Linear));

        }
        if (BehaviourType == PlatformBehaviour.rotatingZ)
        {
            Vector3 rotationDestination = Vector3.zero;
            rotationDestination.z = 360f;

            tweens.Add(transform.DORotate(rotationDestination, RotationDuration, RotateMode.FastBeyond360).SetLoops(-1).SetEase(Ease.Linear));

        }
    }
    void LinearMovementLogic()
    {
        Vector3 currentPosition = transform.position;
        Vector3 Destination = currentPosition + Displacement;
        if (BehaviourType == PlatformBehaviour.LineMove)
        {
            tweens.Add(transform.DOMove(Destination,DisplacementDuration).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear));
        }
    }
    void OneWayLogic()
    {
       
    }
    void FallLogic()
    {
        if (BehaviourType == PlatformBehaviour.FallThrough)
        {
            rb = GetComponent<Rigidbody>();
            rb.isKinematic = true;
            rb.useGravity = false;

        }
    }
    void SwingLogic()
    {
        
        if(BehaviourType == PlatformBehaviour.Swinging)
        {
            Vector3[] points = new Vector3[3];
            points[0] = SwingPoint1.position;
            points[1] = SwingPoint2.position;
            points[2] = SwingPoint3.position;
            //transform.DOMove(Destination,DisplacementDuration).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);
            //transform.DOBlendableMoveBy(new Vector3(1,3,5), 10).SetLoops(-1, LoopType.Yoyo);

            tweens.Add(transform.DOLocalPath(points, SwingSpeed, PathType.CubicBezier).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutCubic)); 
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (BehaviourType == PlatformBehaviour.oneWay) collider.enabled = true;

        else if (BehaviourType == PlatformBehaviour.FallThrough && other.gameObject.CompareTag("Player"))
        {
            //Debug.Log("player Collision");
            RigidBodyActivate(rb);
        }
        else if(BehaviourType == PlatformBehaviour.Speeding && !(other.GetComponent<Movement>()==null))
        {
            other.GetComponent<Movement>().ExtennalSpeedEffctor(SpeedIncrease);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if(BehaviourType == PlatformBehaviour.oneWay) collider.enabled = false;
    }
    private void OnTriggerStay(Collider other)
    {
        if(BehaviourType == PlatformBehaviour.UpWardBlowers && !(other.GetComponent<Movement>() == null))
        {

            other.GetComponent<Movement>()._MoveDestination.y = Mathf.Max(other.GetComponent<Movement>()._MoveDestination.y,-15);
            other.GetComponent<Movement>()._upwardForceActor = Mathf.Min(other.GetComponent<Movement>()._upwardForceActor, 30);
            other.GetComponent<Movement>()._upwardForceActor += upwardsForce *1.4f* Time.fixedDeltaTime;

        }
    }

   
    async void RigidBodyActivate(Rigidbody rb)
    {
        if (rb == null) return;
        await Task.Delay(fallTimer * 1000);
        rb.isKinematic = false;
        rb.useGravity = true;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.HSVToRGB(10,100,10);
        
        
        /*if (BehaviourType == PlatformBehaviour.UpWardBlowers)
        {
            
            Gizmos.DrawLine(DirectionSource.position, transform.position + (DirectionSource.position - transform.position) * 3);
            Physics.Raycast(transform.position, transform.up, 20);
        }*/
         if(BehaviourType == PlatformBehaviour.LineMove)
        {
            //Vector3 postionDif = (transform.position +Displacement) - transform.position;
            
            Gizmos.DrawLine(initialPos, initialPos + Displacement);
            Gizmos.DrawSphere(initialPos, .4f);
            Gizmos.DrawSphere(initialPos + Displacement, .4f);

            
        }
    }
    private void OnDestroy()
    {
        foreach (Tween t in tweens) {
            t.Kill();
        }
    }


}
