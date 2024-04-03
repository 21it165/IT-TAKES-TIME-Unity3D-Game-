using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIntract : MonoBehaviour
{
    
    public GameObject cam;
    [SerializeField]private float distence = 3f;
    [SerializeField]private LayerMask mask;
    [SerializeField]private LayerMask wallmask;
    private PlayerUi playerUi;
    // Start is called before the first frame update
    void Start()
    {
        playerUi = GetComponent<PlayerUi>();
    }

    // Update is called once per frame
    void Update()
    {
        playerUi.UpdateText(string.Empty);
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        Debug.DrawRay(ray.origin, ray.direction * distence);
        RaycastHit hitInfo;
        if(Physics.Raycast(ray, out hitInfo, distence, mask)){
             if(hitInfo.collider.GetComponent<intractable>() != null){
                   intractable intract = hitInfo.collider.GetComponent<intractable>();
                    playerUi.UpdateText(intract.PromptMassage);

                    if(Input.GetButtonDown("E")){
                              intract.BaseIntract();
                    }
             }
        }

        // if(Physics.Raycast(ray, out hitInfo, distence, wallmask)){
        //          playerUi.UpdateText("PRESS Q");
        //             if(Input.GetButtonDown("Q")){
        //                    transform.localRotation = Quaternion.Euler(90f, 0f, 0f);
        //             }
        // }
    }
}
