using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class keypad : intractable
{
    [SerializeField]
    private GameObject door;
    private bool doorOpen;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    protected override void Intract()
    {
       doorOpen = !doorOpen;
       door.GetComponent<Animator>().SetBool("IsOpen", doorOpen);
    }
}
