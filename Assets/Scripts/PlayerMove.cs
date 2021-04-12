using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    //public
    public float speed = 1f;

    //private
    private SpriteRenderer _spriteRenderer;
    private Animator _animator;

    private float _horizontal;

    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
    }

    void Update()
    {
        _horizontal = Input.GetAxisRaw("Horizontal");
        AnimationUpdate();
        MoveUpdate();
    }

    private void AnimationUpdate()
    {
        if(_horizontal == 0)
            _animator.SetBool("isMove",false);
        else
            _animator.SetBool("isMove",true);

        if (_horizontal == -1)
            _spriteRenderer.flipX = true;

        if (_horizontal == 1)
            _spriteRenderer.flipX = false;
    }

    private void MoveUpdate()
    {
        if (_horizontal == -1)
        {
            transform.position += Vector3.left * (Time.deltaTime * speed);
        }

        if (_horizontal == 1)
        {
            transform.position += Vector3.right * (Time.deltaTime * speed);
        }
    }

}