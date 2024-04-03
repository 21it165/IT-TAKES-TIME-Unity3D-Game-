using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallRuning : MonoBehaviour
{
   [Header("WallRunning")]
   public LayerMask WhatIsWall;
   public LayerMask WhatIsGround;
   public float wallRunForce;
   public float wallJumpUpForce;
   public float wallJumpSideForce; 
   public float wallClimbSpeed;
   public float maxWallRunTime;
   private float wallRunTimer;

   [Header("Input")]
   public KeyCode jumpKey = KeyCode.Space;
   public KeyCode upwardsRunKey = KeyCode.LeftShift;
   public KeyCode downwardsRunKey = KeyCode.LeftControl;
   private bool upwardsRunning;
   private bool downwardsRunning;
   private float horizontalInput;
   private float verticalInput;

   [Header("Detection")]
   public float wallCheckDistance;
   public float minJumpHeight;
   private RaycastHit leftWallHit;
   private RaycastHit rightwallHit;
   private bool wallLeft;
   private bool wallRight;

   [Header("Exiting")]
   private bool exitingWall;
   public float exitWallTime;
   private float exitWallTimer;

   [Header("Gravity")]
   public bool useGravity;
   public float gravityCounterForce;

   [Header("Refrences")]
   public Transform orientation;
   public mouse_look cam;
   private player_movement pm;
   private Rigidbody rb;


   private void Start(){

       rb = GetComponent<Rigidbody>();
       pm = GetComponent<player_movement>();
   }

   private void Update()
   {
        CheckForWall();   
        StateMachine();
   }

   private void FixedUpdate()
   {
      if(pm.wallrunning)
      {
        WallRuningMovement();
      }
   }
   
   private void CheckForWall(){

       wallRight = Physics.Raycast(transform.position, orientation.right, out rightwallHit, wallCheckDistance, WhatIsWall);
       wallLeft =  Physics.Raycast(transform.position, -orientation.right, out leftWallHit, wallCheckDistance, WhatIsWall);
    }

    private bool AboveGround()
    {
        return !Physics.Raycast(transform.position, Vector3.down, minJumpHeight, WhatIsGround);

    }

    private void StateMachine()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        upwardsRunning = Input.GetKey(upwardsRunKey);
        downwardsRunning = Input.GetKey(downwardsRunKey);

        if((wallLeft || wallRight) && verticalInput > 0 && AboveGround() && !exitingWall)
        {

              if(!pm.wallrunning)
              {
                    StartWallRun();
              }

              if(wallRunTimer > 0)
                   wallRunTimer -= Time.deltaTime;
                
              if(wallRunTimer <= 0 && pm.wallrunning)
              {
                exitingWall = true;
                exitWallTimer = exitWallTime;
              }

              if(Input.GetKeyDown(jumpKey)) WallJump();
        }
        else if(exitingWall)
        {
            if(pm.wallrunning)
                StopWallRun();
            
            if(exitWallTimer > 0)
                exitWallTimer -= Time.deltaTime;
            
            if(exitWallTimer <= 0)
                exitingWall = false;
        }
        else 
        {
            if(pm.wallrunning)
            {
                StopWallRun();
            }
        }
    }

    private void StartWallRun()
    {
            pm.wallrunning = true;
             
            wallRunTimer = maxWallRunTime;

            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

            cam.DoFov(90f);
            if(wallLeft) cam.DoTilt(-5f);
            if(wallRight) cam.DoTilt(5f);
    }

    private void WallRuningMovement()
    {
           rb.useGravity = useGravity;
          

          Vector3 wallNormal = wallRight ? rightwallHit.normal : leftWallHit.normal;

          Vector3 wallForward = Vector3.Cross(wallNormal, transform.up);

          if((orientation.forward - wallForward).magnitude > (orientation.forward - -wallForward).magnitude)
               wallForward = -wallForward;

          rb.AddForce(wallForward * wallRunForce, ForceMode.Force);

          if(upwardsRunning)
             rb.velocity = new Vector3(rb.velocity.x, wallClimbSpeed, rb.velocity.z);
          if(downwardsRunning)
             rb.velocity = new Vector3(rb.velocity.x, -wallClimbSpeed, rb.velocity.z);
      

          if(!(wallLeft && horizontalInput > 0) && !(wallRight && horizontalInput < 0))
                rb.AddForce(-wallNormal * 100, ForceMode.Force);
        
          if(useGravity)
              rb.AddForce(transform.up * gravityCounterForce, ForceMode.Force);
    }

    private void StopWallRun()
    {
         pm.wallrunning = false;

         cam.DoFov(80f);
         cam.DoTilt(0f);
         
    }

    private void WallJump()
    {
        exitingWall = true;
        exitWallTimer = exitWallTime;

        Vector3 wallNormal = wallRight ? rightwallHit.normal : leftWallHit.normal;

        Vector3 forceToApply = transform.up * wallJumpUpForce + wallJumpSideForce * wallNormal;

        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(forceToApply, ForceMode.Impulse);
    }
}