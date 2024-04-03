using System.Collections;
using System.Collections.Generic;
using TMPro.Examples;
using UnityEngine;

public class player_movement : MonoBehaviour
{
   [Header("Movement")]
   private float moveSpeed;
   public float walkSpeed;
   public float sprintSpeed;
   public float slideSpeed;
   public float wallRunSpeed;

   private float desiredMoveSpeed;
   private float lastDesiredMoveSpeed;
 
   public float speedIncreaseMultiplier;
   public float slopIncreaseMultiplier;

   public float groundDrag;

  [Header("Jumping")]
   public float jumpForce;
   public float jumpCooldown;
   public float airMultiplier;
   bool readyToJump;

   [Header("Crouching")]
   public float crouchSpeed;
   public float crouchYScal;
   private float startYScal;

   [Header("KeyBinds")]
   public KeyCode jumpKey = KeyCode.Space;
   public KeyCode sprintKey = KeyCode.LeftShift;
   public KeyCode crouchKey = KeyCode.C;

  [Header("Ground Check")]
  public float playerHeight;
  public LayerMask whatIsGround;
  bool grounded;

  [Header("Slop Handling")]
  public float maxSlopAngle;
  private RaycastHit slopHit;
  private bool exitingSlope;

  [Header("Camera Effects")]
  public mouse_look cam;
  public float grappleFov = 95f; 

   public Transform orientation;
   float horizontalInput;
   float verticalInput;
   Vector3 moveDirection;
   Rigidbody rb;

   public MovementState state;

   public enum MovementState{
       walking,
       freeze,
       sprinting,
       crouching,
       wallrunning,
       sliding,
       air
   }

   public bool sliding;
   public bool freeze;
   public bool activeGrapple;
   public bool wallrunning;

    private IEnumerator coroutine;

   private void Start(){
    rb = GetComponent<Rigidbody>();
    rb.freezeRotation = true;
    readyToJump = true;
    startYScal = transform.localScale.y;
  }

  private void Update(){
       // ground check
       grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight*0.5f + 0.8f, whatIsGround);

      MyInput();
      SpeedControl();
      StateHandler();

      //handle drag
      if(grounded && !activeGrapple)
         rb.drag = groundDrag;
      else 
         rb.drag = 0;
  }
 
  private void FixedUpdate(){
    Move_Player();
  }
  private void MyInput(){
    horizontalInput = Input.GetAxisRaw("Horizontal");
    verticalInput = Input.GetAxisRaw("Vertical");

    if(Input.GetKey(jumpKey) && readyToJump && grounded){
      readyToJump = false;

      Jump();

      Invoke(nameof(ResetJump), jumpCooldown);
    }

    if(Input.GetKey(crouchKey)){
         transform.localScale = new Vector3(transform.localScale.x, crouchYScal, transform.localScale.z);
        
      // rb.AddForce(Vector3.down, ForceMode.Impulse);
    }
    if(Input.GetKeyUp(crouchKey)){
         transform.localScale = new Vector3(transform.localScale.x, startYScal, transform.localScale.z);
    }

  }

  private void StateHandler(){

    if(freeze)
    {
        state = MovementState.freeze;
        moveSpeed = 0;
        rb.velocity = Vector3.zero;
    }
    else if(wallrunning)
    {
        state = MovementState.wallrunning;
        desiredMoveSpeed = wallRunSpeed;

    }
    else if(sliding){

      state = MovementState.sliding;

      if(OnSlope() && rb.velocity.y < 0.1f)
          desiredMoveSpeed = slideSpeed;
      else
         desiredMoveSpeed = sprintSpeed;
    }
    else if(Input.GetKey(crouchKey)){
      state = MovementState.crouching;
      desiredMoveSpeed = crouchSpeed;
    }
    else if(grounded && Input.GetKey(sprintKey)){
      state = MovementState.sprinting;
      desiredMoveSpeed = sprintSpeed;
    }
    else if(grounded){
      state = MovementState.walking;
      desiredMoveSpeed = walkSpeed;
    }
    else{
        state = MovementState.air;
    }

    if(Mathf.Abs(desiredMoveSpeed - lastDesiredMoveSpeed) > 4f && moveSpeed != 0)
    {
      StopAllCoroutines();
      coroutine = SmoothlyLerpMoveSpeed();

      StartCoroutine(coroutine);
    }
    else{
      moveSpeed = desiredMoveSpeed;
    }
    lastDesiredMoveSpeed = desiredMoveSpeed;
  }

  private IEnumerator SmoothlyLerpMoveSpeed()
  {
     
      float time = 0;
      float diffrence = Mathf.Abs(desiredMoveSpeed - moveSpeed);
      float startValue = moveSpeed;

      while(time < diffrence){
        moveSpeed = Mathf.Lerp(startValue, desiredMoveSpeed, time / diffrence);
        
        if(OnSlope()){
          float slopAngle = Vector3.Angle(Vector3.up, slopHit.normal);
          float slopAngleIncrease = 1 + (slopAngle / 90f);

          time += Time.deltaTime * speedIncreaseMultiplier * slopIncreaseMultiplier * slopAngleIncrease;

        }
        else 
           time += Time.deltaTime * speedIncreaseMultiplier;

        yield return null;
      }
      moveSpeed = desiredMoveSpeed;
  }

  private void Move_Player(){

    if(activeGrapple) return;

    moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
    
    if(OnSlope() && !exitingSlope){
      rb.AddForce(GetSlopMoveDirection(moveDirection) * moveSpeed * 20f, ForceMode.Force);
      
      if(rb.velocity.y > 0)
          rb.AddForce(Vector3.down * 80f, ForceMode.Force);
    }

    else if(grounded) 
       rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
    else if(!grounded)
        rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
      
    if(!wallrunning) rb.useGravity = !OnSlope();
  }

  private void SpeedControl(){

    if(activeGrapple) return;

    if(OnSlope() && !exitingSlope){
              if(rb.velocity.magnitude > moveSpeed)
                  rb.velocity = rb.velocity.normalized * moveSpeed;
    }
    else{
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        if(flatVel.magnitude > moveSpeed){
            Vector3 limitVel = flatVel.normalized * moveSpeed;
             rb.velocity = new Vector3(limitVel.x, rb.velocity.y, limitVel.z);
         }

    }
    
  }

 private void Jump(){
  rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

  rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);

  exitingSlope = true;
 }

 private void ResetJump(){
  readyToJump = true;
  
  exitingSlope = false;
 }

 private Vector3 velocityToSet;
 private bool enableMovementOnTouch;
 public void JumpToPosition(Vector3 targetPosition, float trajectoryHeight)
 {
  activeGrapple = true;

    velocityToSet = CalculateJumpVelocity(transform.position, targetPosition, trajectoryHeight);
    Invoke(nameof(SetVelocity), 0.1f);

    Invoke(nameof(ResetRestriction), 3f);
 }

 private void SetVelocity()
 {
    enableMovementOnTouch = true;
    rb.velocity = velocityToSet;

    cam.DoFov(grappleFov);
 }

 public void ResetRestriction()
 {
    activeGrapple = false;
    cam.DoFov(85f);
 }

 private void OnCollisionEnter(Collision collision)
 {
    if(enableMovementOnTouch)
    {
        enableMovementOnTouch = false;
        ResetRestriction();

        GetComponent<Grapling>().StopGrapple();
    }
 }
 public bool OnSlope(){
     
     if(Physics.Raycast(transform.position, Vector3.down, out slopHit, playerHeight * 0.5f + 1f)){

        float angle = Vector3.Angle(Vector3.up, slopHit.normal);
        return angle < maxSlopAngle && angle != 0;
     }
     return false;
 }

 public Vector3 GetSlopMoveDirection(Vector3 direction){
  return Vector3.ProjectOnPlane(direction, slopHit.normal).normalized;
 }

 public Vector3 CalculateJumpVelocity(Vector3 startPoint, Vector3 endPoint, float trajectoryHeight)
 {
    float gravity = Physics.gravity.y;
    float dispalcementY = endPoint.y = startPoint.y;
    Vector3 displacementXZ = new Vector3(endPoint.x - startPoint.x, 0f, endPoint.z - startPoint.z);

    Vector3 velocityY = Vector3.up * Mathf.Sqrt(-2 * gravity * trajectoryHeight);
    Vector3 velocityXZ = displacementXZ / (Mathf.Sqrt(-2 * trajectoryHeight / gravity)
                          + Mathf.Sqrt(2 * (dispalcementY - trajectoryHeight) / gravity));

    return velocityXZ + velocityY;
 }
}
