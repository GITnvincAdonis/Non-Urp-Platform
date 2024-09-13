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

        LineRenderer = GetComponent<LineRenderer>();    
         
    }
    void Start()
    {
        RotationLogic();
        LinearMovementLogic();
     
        SwingLogic();
        FallLogic();
    }
    void RotationLogic()
    {
        if (BehaviourType == PlatformBehaviour.rotatingX)
        {
            Vector3 rotationDestination = Vector3.zero;
            rotationDestination.x = 360f + transform.rotation.eulerAngles.x;
            tweens.Add(transform.DORotate(rotationDestination, RotationDuration, RotateMode.FastBeyond360).SetLoops(-1).SetEase(Ease.Linear));
        }
        if (BehaviourType == PlatformBehaviour.rotatingY)
        {
            Vector3 rotationDestination = Vector3.zero;
            rotationDestination.y = 360f + transform.rotation.eulerAngles.y;
            tweens.Add(transform.DORotate(rotationDestination, RotationDuration, RotateMode.FastBeyond360).SetLoops(-1).SetEase(Ease.Linear));
        }
        if (BehaviourType == PlatformBehaviour.rotatingZ)
        {
            Vector3 rotationDestination = Vector3.zero;
            rotationDestination.z = 360f + transform.rotation.eulerAngles.z;
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
            tweens.Add(transform.DOLocalPath(points, SwingSpeed, PathType.CubicBezier).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutCubic)); 
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (BehaviourType == PlatformBehaviour.oneWay) collider.enabled = true;
        else if (BehaviourType == PlatformBehaviour.FallThrough && other.gameObject.CompareTag("Player"))
        {
            RigidBodyActivate(rb);
        }
        else if(BehaviourType == PlatformBehaviour.Speeding && !(other.GetComponent<Movement>()==null))
        {
            other.GetComponent<Movement>().ExtennalSpeedEffctor(SpeedIncrease);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        Debug.Log(other.name);
        collider.enabled = false;
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
   
    private void OnDestroy()
    {
        foreach (Tween t in tweens) {
            t.Kill();
        }
    }
    private LineRenderer LineRenderer;
    private Vector3[] points;
    [SerializeField] private Material material;

    private void SetPoints(Vector3[] points)
    {
        LineRenderer.positionCount = points.Length;
        this.points = points;
    }
    private void OnEnable()
    {
        if(BehaviourType == PlatformBehaviour.LineMove)
        {
            GameObject sphere1= GameObject.CreatePrimitive(PrimitiveType.Sphere);
            GameObject sphere2 = GameObject.CreatePrimitive(PrimitiveType.Sphere);

            sphere1.transform.localScale = new Vector3(.8f,.8f,.8f);
            sphere2.transform.localScale = new Vector3(.8f, .8f, .8f);

            
            sphere1.GetComponent<MeshRenderer>().material = material;
           
            sphere2.GetComponent<MeshRenderer>().material = material;

            sphere1.GetComponent<SphereCollider>().enabled = false;
            sphere2.GetComponent<SphereCollider>().enabled=false;

            Vector3[] postions = new Vector3[2];
            postions[0] = initialPos;
            sphere1.transform.position = initialPos;

            postions[1] = initialPos + Displacement;
            sphere2.transform.position = initialPos + Displacement;

            SetPoints(postions);
            for (int i = 0; i < points.Length; i++)
            {
                LineRenderer.SetPosition(i, points[i]);
            }
        }
      
    }


}
