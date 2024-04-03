using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sliding : MonoBehaviour
{
   [Header("Refrence")]
   public Transform orientation;
   public Transform playerObj;
   private Rigidbody rb;
   private player_movement pm;

   [Header("Sliding")]
   public float maxSlidingTime;
   public float slideForce;
   private float slideTmer;

   public float slideYScale;
   private float startYScale;

   [Header("Input")]
   public KeyCode slideKey = KeyCode.LeftControl;
   private float horizontalInput;
   private float verticalInput;


   private void Start(){

      rb = GetComponent<Rigidbody>();
      pm = GetComponent<player_movement>();

      startYScale = playerObj.localScale.y;
   }

   private void Update(){

      horizontalInput = Input.GetAxisRaw("Horizontal");
      verticalInput = Input.GetAxisRaw("Vertical");

      if(Input.GetKeyDown(slideKey) && ( horizontalInput != 0 || verticalInput !=0 ))
            StartSlide();

      if(Input.GetKeyUp(slideKey) && pm.sliding)
            StopSlide();
   }

   private void FixedUpdate(){

      if(pm.sliding)
        slidingMovement();
   }

   private void StartSlide(){
        pm.sliding = true;

        playerObj.localScale = new Vector3(playerObj.localScale.x, slideYScale, playerObj.localScale.z);
        rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);

        slideTmer = maxSlidingTime;
   }

   private void slidingMovement(){
       
        Vector3 inputDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        if(!pm.OnSlope() || rb.velocity.y > -0.1f){

            rb.AddForce(inputDirection.normalized * slideForce, ForceMode.Impulse);

            slideTmer -= Time.deltaTime;
        }
        else{

          rb.AddForce(pm.GetSlopMoveDirection(inputDirection).normalized * slideForce, ForceMode.Impulse);

        }

        if(slideTmer <= 0)
            StopSlide();
          
   }

   private void StopSlide(){
          
        pm.sliding = false;

         playerObj.localScale = new Vector3(playerObj.localScale.x, startYScale, playerObj.localScale.z);
   }

}
