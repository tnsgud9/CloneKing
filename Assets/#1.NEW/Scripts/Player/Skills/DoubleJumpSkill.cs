using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleJumpSkill : BaseSkill
{
    private PlayerJump _playerJump;

    public void Start()
    {
        coolTime = 10.0f;
    }

    public override void BindPlayerController(PlayerController playerController)
    {
        base.BindPlayerController(playerController);

        if( playerController != null)
        {
            _playerJump = playerController.gameObject.GetComponent<PlayerJump>();
        }
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
        if (_playerJump.DoubleJump())
        {
            return true;
        }

        return base.OnStartAction();
    }
}
