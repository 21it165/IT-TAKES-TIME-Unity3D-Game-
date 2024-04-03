using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grapling : MonoBehaviour
{
   [Header("Refrences")]
   private player_movement pm;
   public Transform cam;
   public Transform gunTip;
   public LayerMask wahtIsGrappleable;
   public LineRenderer lr;

   [Header("Grappling")]
   public float maxGrappleDistance;
   public float grappleDelayTime;
   public float overShootYAxis;

   private Vector3 grapplePoint;

   [Header("Cooldown")]
   public float graplingCd;
   private float grapplingCdTimer;

   [Header("Input")]
   public KeyCode grappleKey = KeyCode.Mouse1;
   private bool grappling;

   private void Start()
   {
       pm = GetComponent<player_movement>();

   }

   private void Update()
   {
      if(Input.GetKeyDown(grappleKey)) StartGrapple();

     

      if(grapplingCdTimer > 0)
           grapplingCdTimer -= Time.deltaTime;
   }

   private void LateUpdate()
   {
       if(grappling)
            lr.SetPosition(0, gunTip.position);
   }
   private void StartGrapple()
   {

        if(grapplingCdTimer > 0) return;

        grappling = true;

        pm.freeze = true;

        RaycastHit hit;
        if(Physics.Raycast(cam.position, cam.forward, out hit, maxGrappleDistance, wahtIsGrappleable))
        {
            grapplePoint = hit.point;

            Invoke(nameof(ExecuteGrapple), grappleDelayTime);
        }
        else
        {
            grapplePoint = cam.position + cam.forward * maxGrappleDistance;

            Invoke(nameof(StopGrapple), grappleDelayTime);
        }

        lr.enabled = true;
        lr.SetPosition(1, grapplePoint);
   }

   private void ExecuteGrapple()
   {
        pm.freeze = false;

        Vector3 lowestPoint = new Vector3(transform.position.x, transform.position.y - 1f, transform.position.z);

        float grapplePointRelativeYPos = grapplePoint.y - lowestPoint.y;
        float highestPointOnArc = grapplePointRelativeYPos + overShootYAxis;

        if(grapplePointRelativeYPos < 0) highestPointOnArc = overShootYAxis;

        pm.JumpToPosition(grapplePoint, highestPointOnArc);

        Invoke(nameof(StopGrapple), 1f);
   }

   public void StopGrapple()
   {
        grappling = false;

        pm.freeze = false;

        grapplingCdTimer = graplingCd;

        lr.enabled = false;
   }



}

