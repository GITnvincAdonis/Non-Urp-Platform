using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;
public class CannonController : MonoBehaviour, IAttachable
{
    [SerializeField] private float rotationStep;


    private CANNON cannonControls;
    bool activatedQuestion = false;

    // Start is called before the first frame update
    private void Awake()
    {
        cannonControls = new CANNON();

        cannonControls.CannonControls.WASD.started += RotationEnabled;
        cannonControls.CannonControls.WASD.performed += RotationEnabled;
        cannonControls.CannonControls.WASD.canceled += RotationEnabled;


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
            Debug.Log(Direction);
            Quaternion currentRotation = transform.rotation;
            Quaternion targetRotation = Quaternion.LookRotation(Direction);


            transform.DORotate(targetRotation.eulerAngles, rotationStep);


        }
        if (activatedQuestion) {
            Debug.Log("Ativated and rotating");
        }
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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

}
