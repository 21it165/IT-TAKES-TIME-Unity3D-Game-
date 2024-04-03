using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animation_gun : MonoBehaviour
{
   // public GameObject gun;
    

    public KeyCode anim_gun = KeyCode.Y;

    // Start is called before the first frame update
   public void Start()
    {
      // anim = gun.GetComponent<Animation>();
    }

    // Update is called once per frame
    public void Update()
    {
        if(Input.GetKeyDown(anim_gun))
        {
            GetComponent<Animation>().Play("gun");
        }
    }
}
