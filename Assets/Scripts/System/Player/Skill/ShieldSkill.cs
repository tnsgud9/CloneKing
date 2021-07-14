using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldSkill : BaseSkill
{
    public void Start()
    {
        delayTime = 5.0f;
        coolTime = 5.0f;
    }


    protected override void OnFinishDelayAction()
    {
        base.OnFinishDelayAction();
    }

    protected override void OnActivation()
    {
        base.OnActivation();
    }

    protected override bool OnStartAction()
    {
        base.OnStartAction();

        return true;
    }
}
