using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;
using System.Threading.Tasks;
using Unity.VisualScripting;
using DG.Tweening.Core.Easing;


enum States
{
    GroundState,
    JumpState,
    fallingState,
    SlideState,
    LauchState,
    LauchingState,
    SlideOff,

}
public class Movement : MonoBehaviour
{
    [SerializeField] private CONTROLSASSET _inputActions;
    [SerializeField] private UserInterfaceSO _userInterface;
    public List<CannonController> CANNONS = new List<CannonController>();
    Dictionary<CannonController, float> CANNONSAndTime = new Dictionary<CannonController, float>();
    public CharacterController _controller;
    [SerializeField] Animator _animator;

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
    [SerializeField] public float _moveMentSpeed;
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
    RaycastHit ray; 
    RaycastHit midPointHit, frontPointHit;

    [SerializeField] CannonMoveRelay _CannonMoveRelay;





    [SerializeField] private ParticleSystem _runParticles;
    [SerializeField] private ParticleSystem _collisionParticles;

    [SerializeField] private AudioEventSO _audioEventSO;


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


       
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

    }
    void Start()
    {
        SlideColliderTrigger(_normalColliderHeight);
    }
    void PauseEnabled(InputAction.CallbackContext context)
    {
        
        _inputActions.MOVES.Disable();
        _inputActions.UI.Enable();
        Cursor.visible = true;



    }
    void UnpauseEnabled(InputAction.CallbackContext context) {
        
        _inputActions.MOVES.Enable();
        _inputActions.UI.Disable();
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
    }
    void MovementAssignment(InputAction.CallbackContext context)
    {
        Vector2 directionDestinations = context.ReadValue<Vector2>();
        _MoveDestination.x = directionDestinations.x;
        _MoveDestination.z = directionDestinations.y;

        if (directionDestinations.x == 0 && directionDestinations.y == 0)
        {
            _isMoving = false;
            _animator.SetBool("IsMoving", false);
        }
        else
        {
            _isMoving = true;
            _animator.SetBool("IsMoving", true);
        }       
    }
    void jumpActivated(InputAction.CallbackContext context)
    {
        bool jumpStat = context.ReadValueAsButton();
        _JumpPress = jumpStat;
        if(_currentState != States.LauchState) handleJump();
        if(_currentState == States.LauchState)
        {
            ChangeState(States.LauchingState);
            //_currentState = States.LauchingState;
        }
    }
    void SlideActiviated(InputAction.CallbackContext context)
    {
        bool state = context.ReadValueAsButton();
        _isSliding = state;
        //Debug.Log("it");
        if (state && (_currentState == States.GroundState))
        {
            ChangeState(States.SlideState);
            //_currentState = States.SlideState;
            _animator.SetTrigger("SlideTrigger");
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
            ChangeState(States.SlideState);
            //_currentState = States.SlideState;
            SlideColliderTrigger(_slideColliderHeight);
        }
    }
    void InteractEnabled(InputAction.CallbackContext context)
    {
        bool enabled = context.ReadValueAsButton();
        
        if (_currentState == States.LauchingState || _currentState == States.LauchState)
        {
            Debug.Log("unparwnt");
            transform.SetParent(null);
            ChangeState(States.GroundState);
            //_currentState = States.GroundState;
            
        }
      

        else if (_currentState != States.LauchState && _currentlyInInteractZone )
        {
            ChangeState(States.LauchState);
            //_currentState = States.LauchState;
            foreach (CannonController cannon in CANNONS){
                if (cannon != null){
                    //cannon.Attach(gameObject.transform, new Vector3(0, 0, 2));
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
        ChangeState(States.GroundState);
        //_currentState = States.GroundState;

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
            _animator.SetTrigger("JumpTrigger");
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
    protected Vector3 AltGetSlopeMoveDir(RaycastHit slopeHit)
    {
        return Vector3.ProjectOnPlane(transform.forward, slopeHit.normal).normalized;
    }



    // STATE SPECIFIC LOGIC===========================================================================================================================================================================
    void ChangeState(States stateToSwitchTo)
    {
        if (stateToSwitchTo == States.GroundState || stateToSwitchTo == States.JumpState)
        {
            _collisionParticles.Play();
            _audioEventSO.RaiseAudioEventWithVolume(AudioLibrary.instance._LandingSound, transform.position, true,20);
        }
        _currentState = stateToSwitchTo;
    }
    void GroundStateConditions(){
        _JumpCount = 0;
        HandleRotation(_turnFactor);
        HandleGroundGravity(); 
        if (_JumpPress){
           // Debug.Log("grounded, jumping");
            ChangeState(States.JumpState);
        }
        if (!_controller.isGrounded)
        {

            _animator.SetBool("IsFallingDown", true);
        }
        if (_controller.isGrounded) {
            _animator.SetBool("IsFallingDown", false);
        }
    }

    void JumpStateConditions(){
        HandleRotation(_turnFactor);
        HandleGravity();
        if (_controller.isGrounded){
            //Debug.Log("grounded here");
            ChangeState(States.GroundState);
        }
        
        
    }
    void SlideStateConditions()
    {
        HandleRotation(_slideTurnFactor);
        HandleGroundGravity();

        Vector3 slideDirection = AltGetSlopeMoveDir(ray);
        _SlideDestination.x = transform.forward.x * _moveMentSpeed;
        _SlideDestination.y += _NormalGravity;
        _SlideDestination.z = transform.forward.z * _moveMentSpeed;
        _controller.Move(_SlideDestination * Time.fixedDeltaTime);
        if (_JumpPress)
        {
            _animator.SetTrigger("JumpTrigger");
            
            _SlideDestination.y = 0;
            SlideColliderTrigger(_normalColliderHeight);
            ChangeState(States.JumpState);
            StopForceSlide();
        }
        if (!_isMoving)
        {
            
            _SlideDestination.y = 0;
            
            ChangeState(States.GroundState);
            SlideColliderTrigger(_normalColliderHeight);
            StopForceSlide();
        }

        if (_moveMentSpeed < _slideStateCancelSpeed)
        {
            
            _SlideDestination.y = 0;
            SlideColliderTrigger(_normalColliderHeight);
            _currentState = States.GroundState;
        }

    }
    void LaunchingStateConditions()
    {
        //Debug.Log("flying");
    }
    public void ExtennalSpeedEffctor(float speed) { 
        _moveMentSpeed += speed;
    }
    Coroutine walkRoutine;
    IEnumerator walkSound() {
        
        float waitTime = 1.5f/_moveMentSpeed;
        Debug.Log(waitTime);
        if (waitTime < 1) yield return new WaitForSeconds(waitTime);
        else yield return new WaitForSeconds(2);
        _audioEventSO.RaiseAudioEventWithVolume(AudioLibrary.instance._walk, transform.position, false, 0.05f);
        walkRoutine = null;
    }

    public bool canPlayWalkAudio;
    void FixedUpdate()
    {
        if (_isMoving && _controller.isGrounded && _currentState == States.GroundState)
        {
            canPlayWalkAudio = true;
        }
        else { 
            canPlayWalkAudio = false;
        }
        float mag = new Vector3(_MoveDestination.x * _moveMentSpeed, 0, _MoveDestination.z * _moveMentSpeed).magnitude;
        
        float blendFactor = Mathf.Clamp((_moveMentSpeed), 0.0f, 15f);
        //Debug.Log(blendFactor);
        _animator.SetFloat("Blend", blendFactor);
        _animator.SetBool("IsGrounded", _controller.isGrounded);
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
            _animator.SetBool("IsSliding", true);
            SlideStateConditions();

        }
        else
        {
            _animator.SetBool("IsSliding", false);
        }
         if (_currentState == States.LauchingState)
        {

            LaunchingStateConditions();
        }

        foreach (CannonController cannon in CANNONS)
        {
            if (cannon != null && _currentState == States.LauchState)
            {
                cannon.Attach(gameObject.transform, new Vector3(0, 0, 2));
                //cannon.Interact(true);

            }
        }
        _moveMentSpeed = Mathf.Clamp(_moveMentSpeed, 4, 45);
        _userInterface.ChangeSpeedBar(_moveMentSpeed, transform.position.x, transform.position.y, transform.position.z);
        _upwardForceActor = Mathf.Max(0, _upwardForceActor + _NormalGravity);
        _MoveDestination.y = Mathf.Max(-25, _MoveDestination.y);




        if(_moveMentSpeed >= 10 && _controller.isGrounded && _isMoving)
        {
            _runParticles.Play();
        }
        else
        {
            _runParticles.Stop();
        }

       

    }


 



    //BOUNCE PLATFORM INTERACTIONS
    private void OnControllerColliderHit(ControllerColliderHit hit){
        if (hit.gameObject.CompareTag("Bouncy")){
            Debug.Log("bouncy touch");
            _MoveDestination.y = _initialVelocity * 2;
            ToggleJumpState();
        }
        if (_currentState == States.LauchingState) {
            //Debug.Log("unparwnt");
            transform.SetParent(null);
            ChangeState(States.GroundState);
        }
    }
    
    async void ToggleJumpState()
    {
        await Task.Delay(100);
        ChangeState(States.JumpState);
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


    void AcceptCannonPathCoord(float[] coords)
    {
        if(_currentState == States.LauchingState) _controller.Move(new Vector3(coords[0]* .8f, coords[1], coords[2])* Time.fixedDeltaTime );
    }
    private void OnEnable()
    {
        _CannonMoveRelay.CannonPathCoords.AddListener(AcceptCannonPathCoord);
        _inputActions.MOVES.Enable();
        _inputActions.UI.Enable();
    }
    private void OnDisable()
    {
        _userInterface.Resethealth();
        _CannonMoveRelay.CannonPathCoords.RemoveListener(AcceptCannonPathCoord);
        _inputActions.MOVES.Disable();
        _inputActions.UI.Disable();
    }
    
}

