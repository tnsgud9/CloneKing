using System.Collections;

using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float speed = 1f;

    void Start()
    {
        
    }

    void Update()
    {
        int h = (int)Input.GetAxis("Horizontal");

        transform.Translate(h*Time.deltaTime*speed, 0, 0);
        Debug.Log(h);
        
    }
}
