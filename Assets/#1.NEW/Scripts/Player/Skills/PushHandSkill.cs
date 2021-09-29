using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushHandSkill : MonoBehaviour , ISkill
{
    private SpriteRenderer _spriteRenderer;
    private PlayerJump _playerJump;
    private PlayerMove _playerMove;
    private Animator _animator;
    public GameObject pushHand;
    public float pushForce;
    public Sprite playerPushSprite;

    private void Awake()
    {
        
    }

    private void Start()
    {
        
    }

    public int coolTime
    {
        get => coolTime;
        set => coolTime = value;
        
    }

    public void action()
    {
        Debug.Log("Push hand");
    }
}
