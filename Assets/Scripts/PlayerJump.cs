using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    private PlayerMove _playerMove;
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;
    private Rigidbody2D _rigidbody;
    private float _jumpForce;

    public Sprite jumpReadySprite;
    public Sprite jumpSprite;
    public bool isJump = false;
    public bool isGround = true;
    public float jumpMaxForce = 10f;
    public float jumpMinForce = 3f;

    public PhysicsMaterial2D playerPhysicsMaterial;
    public PhysicsMaterial2D playerBouncePhysicsMaterial;

    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
        _playerMove = GetComponent<PlayerMove>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _jumpForce = jumpMinForce;
    }


    void Update()
    {
        
        JumpEvent();
        
    }

    private void JumpEvent()
    {
        #region Jump Ready
        if (Input.GetKey(KeyCode.Space) && isGround && !isJump)
        {
            _playerMove.enabled = false;
            _animator.enabled = false;
            _spriteRenderer.sprite = jumpReadySprite;
            
            _jumpForce += 5f * Time.deltaTime;
            if (_jumpForce > jumpMaxForce) _jumpForce = jumpMaxForce;
        }
        #endregion

        #region Jump
        if (Input.GetKeyUp(KeyCode.Space) && isGround && !isJump)
        {
            isJump = true;
            _spriteRenderer.sprite = jumpSprite;
            if (_spriteRenderer.flipX) // 왼쪽 보고 있을 때 
            {
                _rigidbody.velocity = new Vector2(-1, 1) * _jumpForce;
            }
            else // 오른쪽 보고 있을떄
            {
                _rigidbody.velocity = new Vector2(1, 1) * _jumpForce;
            }

            _rigidbody.sharedMaterial = playerBouncePhysicsMaterial;
            _jumpForce = jumpMinForce;
        }
        #endregion
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
       // Debug.Log("col enter");
        foreach (ContactPoint2D hitPos in other.contacts)
        {
            //Debug.Log("Hit Pos Normal" + hitPos.normal);
            //Debug.Log("Hit Pos y" + hitPos.normal.y);
            
            
            if (hitPos.normal.y >= 0.5f)
            {
                isGround = true;
                isJump = false;
                _playerMove.enabled = true;
                _animator.enabled = true;
                _rigidbody.sharedMaterial = playerPhysicsMaterial;
        
                // 땅에 부딪힐 경우 강제적으로 아래로 힘을 넣어서 관성작용을 멈춤.
                _rigidbody.velocity = Vector2.zero;
                //velocity는 해당 오브젝트의 질량의 법칙을 무시한다
            }

        }
    }
    
    
}


