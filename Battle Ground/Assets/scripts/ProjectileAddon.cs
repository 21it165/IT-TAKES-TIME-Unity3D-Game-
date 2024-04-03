using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileAddon : MonoBehaviour
{
    int Damage;
   private Rigidbody rb;
   private bool tragetHit;

   private void Start()
   {
      rb = GetComponent<Rigidbody>();
   }

   private void OnCollisionEnter(Collision collision)
   {

        if(tragetHit)
           return;
        else 
           tragetHit = true;
        
        if(collision.gameObject.GetComponent<BasicEnemy>() != null)
        {
            BasicEnemy enemy = collision.gameObject.GetComponent<BasicEnemy>();

            enemy.TakeDamage(Damage);

            Destroy(gameObject);
        }

        rb.isKinematic = true;

        transform.SetParent(collision.transform);
   }
}
