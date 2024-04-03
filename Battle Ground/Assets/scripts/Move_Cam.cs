using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move_Cam : MonoBehaviour
{
    public Transform CamPos;
    void Update()
    {
        transform.position = CamPos.position;
    }
}
