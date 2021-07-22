using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnSkill : BaseSkill
{
    // Start is called before the first frame update
    void Start()
    {
        coolTime = 20.0f;
        delayTime = 0.0f;
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

    protected override bool OnStartAction()
    {
        base.OnStartAction();

        if( _playerController != null )
        {
            Vector3 spawnPosition = transform.position;
            spawnPosition += _playerController.GetForwardVector2D().ConvertToVector3D() * 0.05f;

            spawnPosition.y = -5.0f;

            if (_playerController.photonView.isMine)
            {
                GameObject go = PhotonNetwork.Instantiate("Prefabs/Object/Tower", spawnPosition, Quaternion.identity, 0);

                if (go != null)
                {
                    go.GetComponent<SynchronizedObject>().SetupObject(_playerController, PhotonNetwork.time);
                }
            }

            return true;
        }

        return false;
    }
}
