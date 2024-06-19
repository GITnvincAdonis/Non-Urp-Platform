using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;
using System.Runtime.InteropServices;


using System.Threading.Tasks;
using UnityEngine.UIElements;
using UnityEngine.AI;

public class CannonController : MonoBehaviour, IAttachable
{
    [SerializeField] private float rotationStep;
    [SerializeField] CannonMoveRelay _moveRelay;
    [SerializeField] LineRenderer _Line;
    [SerializeField] float _step;
    [SerializeField] GameObject[] _lineMesh;

    private CANNON cannonControls;
    bool activatedQuestion = false;
    Vector3 pos;
    Task projectileTask;
    public float tempAng;
    public float YAngle;
    Vector3 Direction;
    public float positionMultiplier;
    private void Awake()
    {
        pos = transform.position;
        cannonControls = new CANNON();
        cannonControls.CannonControls.WASD.started += RotationEnabled;
        cannonControls.CannonControls.WASD.performed += RotationEnabled;
        cannonControls.CannonControls.WASD.canceled += RotationEnabled;
        cannonControls.CannonControls.Release.started += ReleaseEnabled;
        cannonControls.CannonControls.Release.performed += ReleaseEnabled;
        cannonControls.CannonControls.Release.canceled += ReleaseEnabled;


    }

    void RotationEnabled(InputAction.CallbackContext context)
    {
        if (activatedQuestion)
        {
            Vector2 inpDirec = context.ReadValue<Vector2>();


            Direction.x = inpDirec.x;
            Direction.y = -inpDirec.y * 0.51f;
            Direction.z = 0;
            //Debug.Log(Direction);
            Quaternion currentRotation = transform.rotation;
            Quaternion targetRotation = Quaternion.LookRotation(Direction);



        }
        if (!activatedQuestion) {
            Debug.Log("out");
        }
    }

    private void FixedUpdate()
    {
        if (activatedQuestion)
        {
            tempAng -= Input.GetAxis("Vertical") * .2f;
            YAngle += Input.GetAxis("Horizontal") * 0.2f;
            if (Input.GetKey(KeyCode.LeftShift))
            {
                positionMultiplier += Time.deltaTime * 9f;
            }
            else
            {
                positionMultiplier -= Time.deltaTime * 7f;
            }
            positionMultiplier = Mathf.Clamp(positionMultiplier, 0, 20);
            tempAng = Mathf.Clamp(tempAng, 20, 80);
            float angle = tempAng * Mathf.Deg2Rad;
            transform.rotation = Quaternion.Euler(-tempAng + 20, YAngle, transform.rotation.eulerAngles.z);
        }
    }

    void ReleaseEnabled(InputAction.CallbackContext context) {
        if (activatedQuestion) {
            float angle = tempAng * Mathf.Deg2Rad;
            Vector3 _Direction = (transform.position + (transform.forward * (10 + positionMultiplier))) - pos;
            Vector3 groundDir = new Vector3(_Direction.x, 0, _Direction.z);
            Vector3 targetPos = new Vector3(groundDir.magnitude, _Direction.y, 0);
            float v0;
            float time;
            CalculatePath(targetPos, angle, out v0, out time);
            StopAllCoroutines();
            StartCoroutine(CoroutineMovement(groundDir.normalized, v0, angle, time));
            activatedQuestion = false;
        }

    }
    private void DrawPath(Vector3 Direction, float v0, float angle, float time, float step)
    {
        step = Mathf.Max(0.01f, step);
        _Line.positionCount = (int)(time / step) + 2;

        int count = 0;
        for (float i = 0; i < time; i += step)
        {
            float x = v0 * i * Mathf.Cos(angle);
            float y = v0 * i * Mathf.Sin(angle) - (1f / 2f) * -Physics.gravity.y * Mathf.Pow(i, 2);
            //_lineMesh[count].transform.position = pos + Direction * x + Vector3.up * y;
            _Line.SetPosition(count, pos + Direction * x + Vector3.up * y);

            count++;
        }
        float xFinal = v0 * time * Mathf.Cos(angle);
        float yFinal = v0 * time * Mathf.Sin(angle) - (1f / 2f) * -Physics.gravity.y * Mathf.Pow(time, 2);
        _Line.SetPosition(count, pos + Direction * xFinal + Vector3.up * yFinal);
        _Line.SetPosition(count, gameObject.transform.position);
    }
    private void CalculatePath(Vector3 targetPos, float angle, out float v0, out float time)
    {
        float xt = targetPos.x;
        float yt = targetPos.y;
        float g = -Physics.gravity.y;

        float v1 = Mathf.Pow(xt, 2) * g;
        float v2 = 2 * xt * Mathf.Sin(angle) * Mathf.Cos(angle);
        float v3 = 2 * yt * Mathf.Pow(Mathf.Cos(angle), 2);
        v0 = Mathf.Sqrt(v1 / (v2 - v3));
        time = xt / (v0 * Mathf.Cos(angle));
    }
    IEnumerator CoroutineMovement(Vector3 Direction, float v0, float angle, float time) {
        float t = 0;
        while ((t < 100)) {
            float x = v0 * t * Mathf.Cos(angle);
            float y = v0 * t * Mathf.Sin(angle) - (1f / 2f) * -Physics.gravity.y * Mathf.Pow(t, 2);

            Vector3 playerPathCoord = Vector3.zero;
            float[] floats = new float[3];
            playerPathCoord = Direction * x + Vector3.up * y;// new Vector3(x,y,0);
            floats[0] = playerPathCoord.x;
            floats[1] = playerPathCoord.y;
            floats[2] = playerPathCoord.z;
            _moveRelay.TransmitPositionPath(floats);
            t += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }
    }
    async Task Movement(Vector3 Direction, float v0, float angle, float time) {
        float t = 0;
        while ((t < 100)) {
            float x = v0 * t * Mathf.Cos(angle);
            float y = v0 * t * Mathf.Sin(angle) - (1f / 2f) * -Physics.gravity.y * Mathf.Pow(t, 2);

            Vector3 playerPathCoord = Vector3.zero;
            float[] floats = new float[3];
            playerPathCoord = Direction * x + Vector3.up * y;// new Vector3(x,y,0);
            floats[0] = playerPathCoord.x;
            floats[1] = playerPathCoord.y;
            floats[2] = playerPathCoord.z;
            _moveRelay.TransmitPositionPath(floats);
            t += Time.fixedDeltaTime;
            await Task.Delay(10);
        }
        projectileTask = null;
    }
    public void Attach(Transform child, Vector3 point)
    {
        child.SetParent(gameObject.transform, false);
        child.rotation = Quaternion.Euler(Vector3.zero);
       
        child.localPosition= point;
        //child.localPosition = point;
    }
    public void  Detach(Transform child){
        child.SetParent(null);
    }
    public void Interact(bool state) {
        //transform.DOLocalMove(new Vector3(0, 10, 0), 10);
        activatedQuestion = state;
    }
    private void OnEnable()
    {
        cannonControls.CannonControls.Enable();
    }
    private void OnDisable()
    {
        cannonControls.CannonControls.Disable();
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        for (int i = 0; i < 10; i++) { 
            Gizmos.DrawSphere(transform.position + transform.forward * (1 + i), .3f );

        }
    }

}
