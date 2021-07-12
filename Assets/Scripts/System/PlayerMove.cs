using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerMove : MonoBehaviour
{
    //public
    public float speed = 1f;
    public AudioClip walkSound;

    //private
    private SpriteRenderer _spriteRenderer;
    private Animator _animator;
    private Rigidbody2D _rigidbody2D;
    private AudioSource _audioSource;
    
    private float _horizontal;

    void Start()
    {
        InitializeComponents();
    }

    private void InitializeComponents()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _audioSource = GetComponent<AudioSource>();
    }


    //MoveEvent는 PlayerController에서 호출됨.
    public void MoveEvent(float horizontal)
    {
        if (enabled)
        {
            MoveUpdate(horizontal);
            AnimationUpdate(horizontal);
        }
    }

    private void AnimationUpdate(float horizontal)
    {
        if(horizontal == 0)
            _animator.SetBool("isMove",false);
        else
            _animator.SetBool("isMove",true);

        if (horizontal == -1)
            _spriteRenderer.flipX = true;

        if (horizontal == 1)
            _spriteRenderer.flipX = false;
    }
    
    private void MoveUpdate(float horizontal)
    {
        if (horizontal == -1)
        {
            transform.position += Vector3.left * (Time.deltaTime * speed);
            if (!_audioSource.isPlaying)
            {
                _audioSource.clip = walkSound;
                _audioSource.PlayDelayed(0.2f);
            }

            return;
        }

        if (horizontal == 1)
        {
            transform.position += Vector3.right * (Time.deltaTime * speed);
            if (!_audioSource.isPlaying)
            {
                _audioSource.clip = walkSound;
                _audioSource.PlayDelayed(0.2f);
            }

            return;
        }
        
        if(_audioSource.isPlaying && _audioSource.name =="walk")
            _audioSource.Stop();
    }

}