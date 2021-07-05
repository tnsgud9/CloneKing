using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;

public class SynchronizedObject : Photon.PunBehaviour
{
    private double _destroyTime = 15.0f;


    public void SetupObject(double destroyTime  )
    {
        _destroyTime = destroyTime;
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if( PhotonNetwork.time >= _destroyTime )
        {
            Destroy(this.gameObject);
        }
    }
}
