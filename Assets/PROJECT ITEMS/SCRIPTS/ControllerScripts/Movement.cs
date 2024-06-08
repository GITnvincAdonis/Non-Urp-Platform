using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;
using System.Threading.Tasks;
using Unity.VisualScripting;


enum States
{
    GroundState,
    JumpState,
    fallingState,
    SlideState,
    LauchState,

}
public class Movement : MonoBehaviour
{
    [SerializeField] private CONTROLSASSET _inputActions;
    [SerializeField] private UserInterfaceSO _userInterface;
    public List<CannonController> CANNONS = new List<CannonController>();
    Dictionary<CannonController, float> CANNONSAndTime = new Dictionary<CannonController, float>();
    public CharacterController _controller;

    [SerializeField] PauseManager _pauseManager;





    public Vector3 _MoveDestination;
    Vector3 _SlideDestination;
    bool _isGrounded;
    bool _isMoving;
    bool _isSliding;
    bool _isCrouching;
    bool _currentlyInInteractZone;
    Vector3 ImpDest;
    int _JumpCount;
    States _currentState;

    private float _jumptimeTemp;
    private float _jumptimeTemp2;
    [SerializeField] private bool _JumpPress;
    [SerializeField] private float _jumpTime;
    [SerializeField] private float _NormalGravity;
    [SerializeField] private float _initialVelocity;
    [SerializeField] private float _maxJumpHeight;
    [SerializeField] private float _moveMentSpeed;
    [SerializeField] private float _moveMentSpeedincrement;
    [SerializeField] private float _turnFactor;
    [SerializeField] private float _slideTurnFactor;
    [SerializeField] private float _cyoteTiming;
    [SerializeField] private float _slideColliderHeight;
    [SerializeField] private float _normalColliderHeight;
    [SerializeField] private float _baseMoveMultiplier;
    [SerializeField] private float _slideStateCancelSpeed;



    public float ExternalSpeedActor;
    public float _upwardForceActor;
    Coroutine _slideRoutine;


    

    // Start is called before the first frame update
    private void Awake()
    {
        _currentState = States.GroundState;
        InitializeJump();
        _inputActions = new CONTROLSASSET();
        _inputActions.MOVES.WASD.started += MovementAssignment;
        _inputActions.MOVES.WASD.performed += MovementAssignment;
        _inputActions.MOVES.WASD.canceled += MovementAssignment;


        _inputActions.MOVES.Jump.started += jumpActivated;

        _inputActions.MOVES.Jump.canceled += jumpActivated;


        _inputActions.MOVES.SLIDE.started += SlideActiviated;
        _inputActions.MOVES.SLIDE.performed += SlideActiviated;
        _inputActions.MOVES.SLIDE.canceled += SlideActiviated;

        _inputActions.MOVES.Crouch.started += CrouchActivated;
        _inputActions.MOVES.Crouch.performed += CrouchActivated;
        _inputActions.MOVES.Crouch.canceled += CrouchActivated;

        _inputActions.MOVES.Interact.started += InteractEnabled;
        _inputActions.MOVES.Interact.performed+= InteractEnabled;
        _inputActions.MOVES.Interact.canceled += InteractEnabled;

        _inputActions.MOVES.Pause.started += PauseEnabled;
        _inputActions.MOVES.Pause.performed += PauseEnabled;
        _inputActions.MOVES.Pause.canceled+= PauseEnabled;


        _inputActions.UI.Unpause.started += UnpauseEnabled;
        _inputActions.UI.Unpause.performed += UnpauseEnabled;
        _inputActions.UI.Unpause.canceled += UnpauseEnabled;

    }
    void Start()
    {
        SlideColliderTrigger(_normalColliderHeight);
    }
    void PauseEnabled(InputAction.CallbackContext context)
    {
        
            _inputActions.MOVES.Disable();
            _inputActions.UI.Enable();
        
        
            
       
    }
    void UnpauseEnabled(InputAction.CallbackContext context) {
        _inputActions.MOVES.Enable();
        _inputActions.UI.Disable();
    }
    void MovementAssignment(InputAction.CallbackContext context)
    {
        Vector2 directionDestinations = context.ReadValue<Vector2>();
        _MoveDestination.x = directionDestinations.x;
        _MoveDestination.z = directionDestinations.y;

        if (directionDestinations.x == 0 && directionDestinations.y == 0)
        {
            _isMoving = false;
        }
        else
        {
            _isMoving = true;
        }       
    }
    void jumpActivated(InputAction.CallbackContext context)
    {
        bool jumpStat = context.ReadValueAsButton();
        _JumpPress = jumpStat;
        handleJump();
    }
    void SlideActiviated(InputAction.CallbackContext context)
    {
        bool state = context.ReadValueAsButton();
        _isSliding = state;
        //Debug.Log("it");
        if (state && (_currentState == States.GroundState))
        {  
            _currentState = States.SlideState;
            StartForceSlide();
            SlideColliderTrigger(_slideColliderHeight);
        }
    }

    void CrouchActivated(InputAction.CallbackContext context)
    {
        bool crouchContext = context.ReadValueAsButton();
        _isCrouching = crouchContext;

        if (_isCrouching && (_currentState == States.JumpState))
        {
            float currentYVelocity = _MoveDestination.y;
            _MoveDestination.y = -6;
        }
        else if (_isCrouching && (_currentState == States.GroundState) && (_moveMentSpeed >= 5))
        { 
            _currentState = States.SlideState;
            SlideColliderTrigger(_slideColliderHeight);
        }
    }
    void InteractEnabled(InputAction.CallbackContext context)
    {
        bool enabled = context.ReadValueAsButton();
        Debug.Log("attempting to interact");
        if (_currentState == States.LauchState && _currentlyInInteractZone)
        {
            _currentState = States.GroundState;
            foreach (CannonController cannon in CANNONS){
                if (cannon != null){
                    cannon.Interact(false);     
                }
            }
        }

        else if (_currentState != States.LauchState && _currentlyInInteractZone)
        {
            _currentState = States.LauchState;
            foreach (CannonController cannon in CANNONS){
                if (cannon != null){
                    cannon.Interact(true);   
                }
            }
        }
    }



    // UTILITIES =============================================================================================================================================================================================
    
    void InitializeJump()
    {
        float timeToApex = _jumpTime * 0.5f;
        _NormalGravity = (-2 * _maxJumpHeight) / Mathf.Pow(timeToApex, 2);
        _initialVelocity = (2 * _maxJumpHeight) / timeToApex;
        _jumptimeTemp = _jumpTime;
        _jumptimeTemp2 = _jumpTime;
    }
    IEnumerator ForcedSlide(){
        _moveMentSpeed = 25;
        while (_moveMentSpeed > _baseMoveMultiplier)
        {
            _moveMentSpeed -= .05F;
            yield return new WaitForEndOfFrame();
        }
        SlideColliderTrigger(_normalColliderHeight);
        StopForceSlide();
        _currentState = States.GroundState;

    }
    void StartForceSlide(){
        if (_slideRoutine == null){
            _slideRoutine = StartCoroutine(ForcedSlide());
        }
    }

    void StopForceSlide(){
        StopCoroutine(_slideRoutine);
        //_moveMentSpeed = _slideStateCancelSpeed * 2 ;
        _slideRoutine = null;
    }
    void handleJump(){

        if (_JumpPress && _controller.isGrounded){
            _MoveDestination.y = _initialVelocity;
            _JumpCount++;
        }
        if (_JumpPress && _JumpCount < 1){
            _JumpCount++;
            _MoveDestination.y = _initialVelocity;

        }
    }
    void HandleGravity(){
        _MoveDestination.y += _NormalGravity;
    }
    void HandleGroundGravity(){
        _MoveDestination.y += _NormalGravity * 0.12f;
    }
    void SlideColliderTrigger(float height)
    {
        _controller.height = height;
    }

    void HandleRotation(float slerpValue){
        if (_isMoving){
            Quaternion currentRotation = _controller.transform.rotation;
            Vector3 targetPos;
            Vector3 Inputs = _MoveDestination;
            targetPos.x = Inputs.x;
            targetPos.y = 0;
            targetPos.z = Inputs.z;
            Quaternion targetRotation = Quaternion.LookRotation(targetPos);
            _controller.transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, slerpValue * Time.fixedDeltaTime);
        }

    }




    // STATE SPECIFIC LOGIC===========================================================================================================================================================================

    void GroundStateConditions(){
        _JumpCount = 0;
        HandleRotation(_turnFactor);
        HandleGroundGravity(); 
        if (_JumpPress){
           // Debug.Log("grounded, jumping");
            _currentState = States.JumpState;
        }
    }

    void JumpStateConditions(){
        HandleRotation(_turnFactor);
        HandleGravity();
        if (_controller.isGrounded){
            //Debug.Log("grounded here");
            _currentState = States.GroundState;
        }
    }
    void SlideStateConditions(){
        HandleRotation(_slideTurnFactor);
        HandleGroundGravity();
        _SlideDestination.x = transform.forward.x * _moveMentSpeed;  
        _SlideDestination.y += _NormalGravity ;
        _SlideDestination.z = transform.forward.z * _moveMentSpeed;
        _controller.Move(_SlideDestination * Time.fixedDeltaTime);
        if (_JumpPress){
            _SlideDestination.y = 0;
            SlideColliderTrigger(_normalColliderHeight);
            _currentState = States.JumpState;
            StopForceSlide();
        }
        if (!_isMoving){
            _SlideDestination.y = 0;
            _currentState = States.GroundState;
            SlideColliderTrigger(_normalColliderHeight);
            StopForceSlide();
        }

        if (_moveMentSpeed < _slideStateCancelSpeed){
            _SlideDestination.y = 0;
            SlideColliderTrigger(_normalColliderHeight);
            _currentState = States.GroundState;
        }

    }

    public void ExtennalSpeedEffctor(float speed) { 
        _moveMentSpeed += speed;
    }
    void FixedUpdate()
    {
        InitializeJump();
        
        ImpDest.x = _MoveDestination.x * _moveMentSpeed;
        ImpDest.y = _MoveDestination.y + _upwardForceActor;
        ImpDest.z = _MoveDestination.z * _moveMentSpeed;


        if (_isMoving)
        {
            _moveMentSpeed = Mathf.SmoothStep(_moveMentSpeed, 9f, .021f);
            _jumpTime = Mathf.Min(_jumptimeTemp + (_jumptimeTemp * _moveMentSpeed * .9f) * Time.fixedDeltaTime, 120);
            //_moveMentSpeed = Mathf.Min(interpolated, 4f);
        }

        else if (!_isMoving)
        {
            _moveMentSpeed = Mathf.SmoothStep(_moveMentSpeed, 1f, .021f);
            _jumpTime = 100;
        }

        if (_currentState == States.JumpState)
        {
            JumpStateConditions();
            _controller.Move(ImpDest * Time.fixedDeltaTime);
        }
        if (_currentState == States.GroundState)
        {
            GroundStateConditions();
            _controller.Move(ImpDest * Time.fixedDeltaTime);
        }
        if (_currentState == States.SlideState)
        {
            SlideStateConditions();

        }
        if (_currentState == States.LauchState)
        {

        }
        _userInterface.ChangeSpeedBar(_moveMentSpeed);
        _upwardForceActor = Mathf.Max(0, _upwardForceActor + _NormalGravity);
        _MoveDestination.y = Mathf.Max(-25, _MoveDestination.y);

       

    }


 



    //BOUNCE PLATFORM INTERACTIONS
    private void OnControllerColliderHit(ControllerColliderHit hit){
        if (hit.gameObject.CompareTag("Bouncy")){
            Debug.Log("bouncy touch");
            _MoveDestination.y = _initialVelocity * 2;
            ToggleJumpState();
        } 
    }
    
    async void ToggleJumpState()
    {
        await Task.Delay(100);
        _currentState = States.JumpState;
    }
 

    // HANDLES INTERACTIONS WITH CANNONS
    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<CannonController>() != null){
            if (!CANNONS.Contains(other.GetComponent<CannonController>())){
                CANNONS.Add(other.GetComponent<CannonController>());
                CANNONSAndTime.Add(other.GetComponent<CannonController>(), 0.0f);
;            }

            _currentlyInInteractZone = true;
        } 

        //Debug.Log("in trigger");
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<CannonController>() != null){
            if (CANNONS.Contains(other.GetComponent<CannonController>())){
                CANNONS.Remove(other.GetComponent<CannonController>());
                CANNONSAndTime.Remove(other.GetComponent<CannonController>());
            }
            _currentlyInInteractZone=false;
            
        }
        //Debug.Log("left trigger");
    }

    private void OnEnable()
    {
        _inputActions.MOVES.Enable();
        _inputActions.UI.Enable();
    }
    private void OnDisable()
    {
        _inputActions.MOVES.Disable();
        _inputActions.UI.Disable();
    }
}

