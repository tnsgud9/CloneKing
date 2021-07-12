using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnSkill : BaseSkill
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected override void OnActivation()
    {
        base.OnActivation();
    }

    protected override void OnFinishDelayAction()
    {
        base.OnFinishDelayAction();
    }

    protected override void OnStartAction()
    {
        base.OnStartAction();

        if( _playerController !=null )
        {
            Vector3 spawnPosition = transform.position;
            spawnPosition += _playerController.GetForwardVector2D().ConvertToVector3D() * 0.25f;

            _playerController.photonView.RPC("RPC_SpawnObject", PhotonTargets.All, "Prefabs/Object/Tower", PhotonNetwork.time, spawnPosition);
        }
    }
}
