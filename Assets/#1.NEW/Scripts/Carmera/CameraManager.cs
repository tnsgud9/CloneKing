using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraManager : Singleton<MonoBehaviour>
{

    /// <summary>
    /// Variable target is affected by playermanager.cs
    /// </summary>
    public Transform target;
    public float smoothCamMove = 2f;

    private void Awake()
    {
        cameraResolution();
    }

    void Update()
    {
        cameraFollow();
        
    }
    
    private void cameraFollow()
    {
        // vec variable would cause a memory leak.
        // Need to refactor later.
        Vector3 vec = target.position;
        vec.z = -10f;
        vec.x = 0f;
        this.transform.position = Vector3.Lerp(transform.position, vec, Time.deltaTime * smoothCamMove);
    }


    private void cameraResolution()
    {
        Camera cam = GetComponent<Camera>();
        Rect rect = cam.rect;
        float heightScale = ((float) Screen.width / Screen.height) / ((float) 9 / 16);
        float widthScale = 1f / heightScale;
        if (heightScale < 1) {
            rect.height = heightScale;
            rect.y = (1f - heightScale) / 2f;
        }
        else
        {
            rect.width = widthScale;
            rect.x = (1f - widthScale) / 2f;
        }
        cam.rect = rect;
    }
}
