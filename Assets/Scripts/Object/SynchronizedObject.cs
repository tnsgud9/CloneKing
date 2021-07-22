using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SynchronizedObject : Photon.PunBehaviour, IPunObservable
{
    public float lifeTime = 15.0f;

    private double _destroyTime = 0.0f;

    protected PlayerController _owner = null;

    public void SetupObject(PlayerController controller, double spawnTime )
    {
        _destroyTime = spawnTime + lifeTime;
        _owner = controller;
    }

    public override void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        base.OnPhotonInstantiate(info);

        info.sender.TagObject = this.gameObject;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (photonView.isMine)
        {
            if (PhotonNetwork.time >= _destroyTime)
            {
                PhotonNetwork.Destroy(gameObject);
            }
        }
    }
}
