using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;
using System.Runtime.InteropServices;


using System.Threading.Tasks;

public class CannonController : MonoBehaviour, IAttachable
{
    [SerializeField] private float rotationStep;
    private CANNON cannonControls;
    bool activatedQuestion = false;
    [SerializeField] float _Angle;
    [SerializeField] float _InitialVelocity;
    Vector3 pos;
    [SerializeField] CannonMoveRelay _moveRelay;
    Task projectileTask;

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
            Vector3 Direction;

            Direction.x = inpDirec.x;
            Direction.y = -inpDirec.y * 0.51f;
            Direction.z = 0;
            //Debug.Log(Direction);
            Quaternion currentRotation = transform.rotation;
            Quaternion targetRotation = Quaternion.LookRotation(Direction);


            transform.DORotate(targetRotation.eulerAngles, rotationStep);


        }
        if (!activatedQuestion) {
            Debug.Log("out");
        }
    }
    void ReleaseEnabled(InputAction.CallbackContext context)
    {

        if (activatedQuestion)
        {
            //finding player


            float tempAngle = Vector3.Angle(transform.forward, Vector3.up);
            float angle = _Angle * Mathf.Deg2Rad;
            Vector3 _Direction = (transform.position + (transform.forward * 4)) - pos;
            Vector3 groundDir = new Vector3(_Direction.x, 0, _Direction.z);
            Vector3 targetPos = new Vector3(groundDir.magnitude, _Direction.y, 0);
            float v0;
            float time;

            CalculatePath(targetPos, angle, out v0, out time);


           StopAllCoroutines();
            StartCoroutine(CoroutineMovement(groundDir.normalized, v0, angle, time));
            //if (projectileTask == null) projectileTask = Movement(groundDir.normalized, v0, angle, time);
            activatedQuestion = false;
        }
        
    }
    private void CalculatePath(Vector3 targetPos, float angle, out float v0, out float time)
    {
        float xt = targetPos.x;
        float yt = targetPos.y;
        float g = -Physics.gravity.y;

        float v1 = Mathf.Pow(xt, 2) * g;
        float v2 = 2 * xt * Mathf.Sin(angle) * Mathf.Cos(angle);
        float v3 = 2 * yt * Mathf.Pow(Mathf.Cos(angle), 2);
        v0 = Mathf.Sqrt(v1/(v2 - v3));
        time = xt/(v0* Mathf.Cos(angle));
    }
    IEnumerator CoroutineMovement(Vector3 Direction, float v0, float angle, float time)
    {
        float t = 0;
        while ((t<100))
        {
            float x = v0 * t * Mathf.Cos(angle);
            float y = v0 * t * Mathf.Sin(angle) - (1f/2f) * -Physics.gravity.y * Mathf.Pow(t,2);

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
    async Task Movement(Vector3 Direction, float v0, float angle, float time)
    {
        float t = 0;
        while ((t < 100))
        {
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
    
    public void Attach(Vector3 position)
    {

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
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position,  transform.forward * 2);
    }

}
