using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;

public class SynchronizedObject : Photon.PunBehaviour
{
    public float lifeTime = 15.0f;

    private double _destroyTime = 0.0f;

    public void SetupObject(double spawnTime )
    {
        _destroyTime = spawnTime + lifeTime;
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
