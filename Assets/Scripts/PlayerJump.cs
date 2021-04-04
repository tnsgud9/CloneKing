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
        // 점프 상태가 아니고 땅에 있을때 
        if (Input.GetKeyDown(KeyCode.Space) && !isJump && isGround) 
        {
            _playerMove.enabled = false; //플레이어 이동 멈춤.
            _animator.enabled = false;
            _spriteRenderer.sprite = jumpReadySprite;
        }

        if (Input.GetKey(KeyCode.Space) && !isJump && isGround)
        {
            _playerMove.enabled = false; //플레이어 이동 멈춤.
            _animator.enabled = false;
            _spriteRenderer.sprite = jumpReadySprite;
            
            _jumpForce += 3f * Time.deltaTime;
            Debug.Log("JUMP FORCE : "+ _jumpForce);
            if (_jumpForce > jumpMaxForce) _jumpForce = jumpMaxForce;
            // 각도 변경 되는 이벤트
        }

        if (Input.GetKeyUp(KeyCode.Space) && !isJump && isGround)
        {
            _playerMove.enabled = false; //플레이어 이동 멈춤.
            _animator.enabled = false;
            _spriteRenderer.sprite = jumpSprite;
            
            if (!_spriteRenderer.flipX) // player see direction right
            {
                _rigidbody.AddForce(new Vector2(1,1) * _jumpForce, ForceMode2D.Impulse);
                //_rigidbody.velocity = new Vector2(1, 1) * _jumpForce;
            }
            else // player see direction left
            {
                _rigidbody.AddForce(new Vector2(-1,1) * _jumpForce, ForceMode2D.Impulse);
                //_rigidbody.velocity = new Vector2(-1, 1) * _jumpForce;
            }

            isJump = true;
            isGround = false;
            
            _rigidbody.sharedMaterial = playerBouncePhysicsMaterial;
            _jumpForce = jumpMinForce;
        }

        //입력 버그 애니메이션 FIX
        if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Move") && !Input.anyKey)
        {
            _animator.SetTrigger("idle");
        }
    }

    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.Space) && !isJump && isGround)
        {
            _playerMove.enabled = false; //플레이어 이동 멈춤.
            _animator.enabled = false;
            _spriteRenderer.sprite = jumpReadySprite;
            
            _jumpForce += Time.deltaTime;
            //Debug.Log("JUMP FORCE : "+ _jumpForce);
            if (_jumpForce > jumpMaxForce) _jumpForce = jumpMaxForce;
            // 각도 변경 되는 이벤트
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        
        //Todo: 수정할 부분
        Debug.Log("Trigger Enter");
        _rigidbody.sharedMaterial = playerPhysicsMaterial;
        _rigidbody.velocity = Vector2.down*1000f;
        //_rigidbody.AddForce(Vector2.down*10f,ForceMode2D.Force);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        isGround = true; // 땅인 경우 isGround = true;
        if (isJump)
        {
            _playerMove.enabled = true;
            _animator.enabled = true;
            isJump = false;
            _rigidbody.sharedMaterial = playerPhysicsMaterial;
        }
        
    }


    private void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log("Collision Enter");
    }

}