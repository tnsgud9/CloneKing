using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldSkill : BaseSkill
{
    private GameObject _originShield = null;
    private GameObject _shieldObject = null;

    public void Start()
    {
        delayTime = 5.0f;
        coolTime = 15.0f;

        _originShield = Resources.Load<GameObject>("Prefabs/Object/PlayerShield");
    }


    protected override void OnFinishDelayAction()
    {
        base.OnFinishDelayAction();

        ChangeLayerMask("Player");

        Destroy(_shieldObject);
    }

    protected override void OnActivation()
    {
        base.OnActivation();
    }

    protected override bool OnStartAction()
    {
        base.OnStartAction();

        ChangeLayerMask("PlayerShield");

        _shieldObject = Instantiate(_originShield, _playerController.transform, false);
        
        return true;
    }

    private void ChangeLayerMask( string layerName)
    {
        gameObject.layer =  LayerMask.NameToLayer(layerName);
    }
}
