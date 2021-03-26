using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    private PlayerMove _playerMove;
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;
    public Sprite jumpReadySprite;
    public Sprite jumpSprite;
    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
        _playerMove = GetComponent<PlayerMove>();
    }

    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _playerMove.enabled = false;
            _animator.enabled = false;
            _spriteRenderer.sprite = jumpReadySprite;
        }
        if (Input.GetKey(KeyCode.Space))
        {
            
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            _spriteRenderer.sprite = jumpSprite;
            _playerMove.enabled = true;
        }
    }


    private void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log("animator online");
        _animator.enabled = true;
    }
}
