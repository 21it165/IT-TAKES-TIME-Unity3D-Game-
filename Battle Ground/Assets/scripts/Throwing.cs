using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throwing : MonoBehaviour
{
    [Header("Refrences")]
    public Transform cam;
    public Transform attackPoint;
    public GameObject objectToThrow;

    [Header("Setting")]
    public int totalThrows;
    public float throwCooldown;

    [Header("Throwing")]
    public KeyCode throwKey = KeyCode.Mouse0;
    public float throwForce;
    public float throwUpawardForce;

    bool readyToThrow;

    void Start(){
        readyToThrow = true;
    }

    private void Update(){

       if(Input.GetKeyDown(throwKey) && readyToThrow && totalThrows > 0){

           Throw();
       }

    }

    private void Throw(){
          
          readyToThrow = false;

          GameObject projectile = Instantiate(objectToThrow, attackPoint.position, cam.rotation);

          Rigidbody projectileRb = projectile.GetComponent<Rigidbody>();

          Vector3 forceDirection = cam.transform.forward;

          RaycastHit hit;

          if(Physics.Raycast(cam.position, cam.forward, out hit, 500f))
          {
            forceDirection = (hit.point - attackPoint.position).normalized;
          }

          Vector3 forceToAdd = forceDirection * throwForce + transform.up * throwUpawardForce;

          projectileRb.AddForce(forceToAdd, ForceMode.Impulse);

          totalThrows--;

          Invoke(nameof(ResetThrow), throwCooldown);

    }

    private void ResetThrow(){

        readyToThrow = true;
    }


}
