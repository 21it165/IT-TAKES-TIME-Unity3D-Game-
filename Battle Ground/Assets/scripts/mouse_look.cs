using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class mouse_look : MonoBehaviour
{
    public float senX = 200f;
    public float senY = 200f;
    public Transform orientation;
    public Transform camHolder;
    float xRotation;
    float yRotation;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;   
    }

    // Update is called once per frame
    void Update()
    {
           float mouseX = Input.GetAxisRaw("Mouse X") * senX * Time.deltaTime;
           float mouseY = Input.GetAxisRaw("Mouse Y") * senY * Time.deltaTime;   

           xRotation -= mouseY;
           yRotation += mouseX;
           xRotation = Mathf.Clamp(xRotation, -90f, 90f);

           camHolder.rotation  = Quaternion.Euler(xRotation, yRotation, 0);
           orientation.rotation = Quaternion.Euler(0, yRotation, 0); 
    }

    public void DoFov(float endValue)
    {
        GetComponent<Camera>().DOFieldOfView(endValue, 0.25f);
    }

    public void DoTilt(float zTilt)
    {
        transform.DOLocalRotate(new Vector3(0, 0, zTilt), 0.25f);
    }
}
