using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfExplosionSkill : BaseSkill
{
    public float explosionWaitForTime = 5.0f;

    private GameObject _originExplosionObject;

    // Start is called before the first frame update
    void Start()
    {
        coolTime = 10.0f;
        delayTime = 0.0f;

        LoadResources();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void LoadResources()
    {
        _originExplosionObject = Resources.Load<GameObject>("Prefabs/Object/ExplosionObject");
    }


    protected override bool OnStartAction()
    {
        base.OnStartAction();

        var explosionObject = Instantiate(_originExplosionObject, transform, false).GetComponent<ExplosionObject>();

        explosionObject.StartExplosion(gameObject, explosionWaitForTime);

        return true;
    }

    protected override void OnFinishDelayAction()
    {
        base.OnFinishDelayAction();
    }

    protected override void OnActivation()
    {
        base.OnActivation();
    }

}
