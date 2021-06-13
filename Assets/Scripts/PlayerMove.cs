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

    public bool reachGoalPoint = false;
    //private
    private SpriteRenderer _spriteRenderer;
    private Animator _animator;
    private Rigidbody2D _rigidbody2D;
    private AudioSource _audioSource;
    
    private float _horizontal;

    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _audioSource = GetComponent<AudioSource>();
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
            //_rigidbody2D.MovePosition(new Vector2(transform.position.x,transform.position.y) + Vector2.left * (Time.deltaTime * speed));
            transform.position += Vector3.left * (Time.deltaTime * speed);
            if (!_audioSource.isPlaying)
            {
                _audioSource.clip = walkSound;
                _audioSource.PlayDelayed(0.2f);
            }

            return;
        }

        if (_horizontal == 1)
        {
            transform.position += Vector3.right * (Time.deltaTime * speed);
            //_rigidbody2D.MovePosition(new Vector2(transform.position.x,transform.position.y) + Vector2.right * (Time.deltaTime * speed));
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