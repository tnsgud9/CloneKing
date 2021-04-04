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


    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
    }

    void Update()
    {
        #region LEFT INPUT

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            _animator.SetTrigger("move");
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            _spriteRenderer.flipX = true;
            transform.position += Vector3.left * (Time.deltaTime * speed);
            if(!_animator.GetCurrentAnimatorStateInfo(0).IsName("Move")) //점프 후 이동중 애니메이션 동작 FIX 
                _animator.SetTrigger("move");
        }


        if (Input.GetKeyUp(KeyCode.LeftArrow))
        {
            if (!Input.GetKey(KeyCode.RightArrow))
            {
                _animator.SetTrigger("idle");
            }
        }

        #endregion

        #region RIGHT INPUT

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            _animator.SetTrigger("move");
            
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            _spriteRenderer.flipX = false;
            transform.position += Vector3.right * (Time.deltaTime * speed);
            if(!_animator.GetCurrentAnimatorStateInfo(0).IsName("Move")) //점프 후 이동중 애니메이션 동작 FIX 
                _animator.SetTrigger("move");
            
        }

        if (Input.GetKeyUp(KeyCode.RightArrow))
        {
            if (!Input.GetKey(KeyCode.LeftArrow))
            {
                _animator.SetTrigger("idle");
            }
        }

        #endregion
    }

}