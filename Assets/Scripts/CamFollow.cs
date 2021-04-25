using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamFollow : MonoBehaviour
{

    public Transform target;
    public float smoothCamMove = 2f;
    
    

    void Update()
    {
        Vector3 vec = target.position;
        vec.z = -10f;
        //vec.x = 0f;
        this.transform.position = Vector3.Lerp(transform.position, vec, Time.deltaTime * smoothCamMove);
    }
}
