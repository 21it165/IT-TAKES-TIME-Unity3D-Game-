using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemy : MonoBehaviour
{
   public int health;

   public void TakeDamage(int Damage){
       health -= Damage;

       if(health <= 0)
           Destroy(gameObject);
   }
   
}
