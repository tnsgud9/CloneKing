using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerPushHand : BaseSkill
{
    private SpriteRenderer _spriteRenderer;
    private PlayerJump _playerJump;
    private PlayerMove _playerMove;
    private Animator _animator;
    public GameObject pushHand;
    public float pushForce;
    public Sprite playerPushSprite;

    
    private void Start()
    {
        coolTime = 10.0f;
        delayTime = 0.7f;
        pushForce = 5.0f;

        InitializeComponents();
        InitializeCharaResource();
    }

    private void InitializeCharaResource()
    {
        pushHand = Resources.Load("Prefabs/Player/Stop Hand") as GameObject;
        playerPushSprite = Resources.Load<Sprite>("Sprites/Characters/VirtualGuy/Stop");

        int charaType;
        if (_playerController != null && _playerController.photonView.TryGetValueToInt("CharaType", out charaType))
        {
            switch ((CharaType)charaType)
            {
                case CharaType.Prince:
                    playerPushSprite = Resources.Load<Sprite>("Sprites/Characters/Prince/Stop");
                    pushHand = Resources.Load("Prefabs/Player/Stop Hand - Shield") as GameObject;
                    break;
            }
        }
    }


    private void InitializeComponents()
    {
        //주의!! player가 갖고 있는 reload bar의 구성이 변경되면 오류가 발생할 수 있음.
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
        _playerJump = GetComponent<PlayerJump>();
        _playerMove = GetComponent<PlayerMove>();
    }

    protected override void OnStartAction()
    {
        base.OnStartAction();

        _animator.enabled = false;
        _playerMove.enabled = false;
        _playerJump.enabled = false;

        _spriteRenderer.sprite = playerPushSprite;

        InstantiatePushHand();
    }

    protected override void OnFinishDelayAction()
    {
        base.OnFinishDelayAction();

        _animator.enabled = true;
        _playerMove.enabled = true;
        _playerJump.enabled = true;
    }

    protected override void OnActivation()
    {
        base.OnActivation();
    }

    private void InstantiatePushHand()
    {
        Vector3 position = transform.position;
        Vector2 direction = Vector2.left;
        Quaternion quaternion = Quaternion.Euler(0, 0, 0);

        if (_spriteRenderer.flipX)
        {
            position.x -= 0.065f;
            quaternion = Quaternion.Euler(0, 180, 0);
        }
        else
        {
            position.x += 0.065f;
            direction = Vector2.right;
        }
        
        GameObject go = Instantiate(pushHand, position, quaternion);

        var collider        = GetComponent<Collider2D>();
        var go_collider     = go.GetComponent<Collider2D>();

        Physics2D.IgnoreCollision(collider, go_collider,true);
        go.GetComponent<Rigidbody2D>().velocity = direction * pushForce;

    }
}
