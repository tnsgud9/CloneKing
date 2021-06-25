using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerJump : MonoBehaviour
{
    private PlayerMove _playerMove;
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;
    private Rigidbody2D _rigidbody;
    private AudioSource _audioSource;
    
    private float _pressTime;
    private JumpState _currentState;

    public float reflectForce = 0.5f;

    public Sprite jumpReadySprite;
    public Sprite jumpSprite;
    
    public PhysicsMaterial2D defaultPhyMat;
    public PhysicsMaterial2D bouncePhyMat;

    public AudioClip jumpSound;
    public AudioClip wallHitSound;
    public AudioClip groundHitSound;

    private void Start()
    {
        InitializeComponents();

        _currentState = JumpState.Ground;
    }

    private void InitializeComponents()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
        _playerMove = GetComponent<PlayerMove>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        DriveJump();
    }

    private void DriveJump()
    {
        switch (_currentState)
        {
            case JumpState.Ready:
                _pressTime += Time.deltaTime;
                break;

            case JumpState.Falling:
                Falling();
                break;
        }
    }

    //JumpEvent는 PlayerController에서 호출됨.
    public bool JumpEvent( JumpState state )
    {
        switch( state)
        {
            case JumpState.Ready:
                if(_currentState.Equals(JumpState.Ground))
                {
                    JumpReady();
                    return true;
                }
                break;

            case JumpState.Jump:
                if(_currentState.Equals(JumpState.Ready))
                {
                    Jump();
                    return true;
                }
                break;

            default:
                return false;
        }

        return false;
    }

    private void Falling()
    {
        const string tileMapLayerName = "TileMap";
        int tileLayerIndex = 1 << LayerMask.NameToLayer(tileMapLayerName);

        if (Physics2D.Raycast(transform.position, Vector2.down, 0.165f, tileLayerIndex))
        {
            Ground();
        }
    }

    private void Ground()
    {
        _currentState = JumpState.Ground;
        
        _rigidbody.sharedMaterial = defaultPhyMat;
        _playerMove.enabled = true;
        _animator.enabled = true;
        _rigidbody.velocity = Vector2.zero;

        _audioSource.clip = groundHitSound;
        _audioSource.Play();
    }

    private void JumpReady()
    {
        _currentState = JumpState.Ready;

        _playerMove.enabled = false;
        _animator.enabled = false;

        _spriteRenderer.sprite = jumpReadySprite;
    }

    private void Jump()
    {
        _currentState = JumpState.Jump;

        _rigidbody.sharedMaterial = bouncePhyMat;
        _playerMove.enabled = false;
        _animator.enabled = false;
        _spriteRenderer.sprite = jumpSprite;

        _audioSource.clip = jumpSound;
        _audioSource.Play();
        
        _pressTime = Mathf.Clamp(_pressTime, 0f, 1f); // 최소 0초에서 최대 1초 동안 점프 기준을 정함
       
        float y = Mathf.Lerp(3f, 7f, _pressTime);
        float x = Mathf.Lerp(1f, 4f, _pressTime);
        
        // 점프 이벤트
        if (_spriteRenderer.flipX) // 왼쪽 보고 있을 때 
        {
            _rigidbody.velocity = new Vector2(-x, y);
        }
        else // 오른쪽 보고 있을떄
        {
            _rigidbody.velocity = new Vector2(x, y);
        }

        _pressTime = 0f;
    }

    public bool isJumped()
    {
        if (_currentState != JumpState.Ground) return true;
        return false;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        _currentState = JumpState.Falling;

        Vector2 contact_normal = other.GetContact(0).normal;
        //_rigidbody.velocity = Vector2.Reflect(-other.relativeVelocity, contact_normal) * reflectForce;

        if (contact_normal.y >= 0.8f)
        {
            Ground();
        }
        else
        {
            _audioSource.clip = wallHitSound;
            _audioSource.Play();
        }

    }
}