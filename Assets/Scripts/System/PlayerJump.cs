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

    private float _maxTime=1f;
    private float _pressTime;
    private JumpState _currentState;
    private bool _canJump = true;
    private bool _playSounds = false;

    public float reflectForce = 0.5f;

    public Sprite jumpReadySprite;
    public Sprite jumpSprite;
    
    public PhysicsMaterial2D defaultPhyMat;
    public PhysicsMaterial2D bouncePhyMat;

    public AudioClip jumpSound;
    public AudioClip wallHitSound;
    public AudioClip groundHitSound;

    public GameObject jumpgauge;
    private Animation _jumpgagueAnimation;
    private void Start()
    {
        InitializeComponents();

        ChangeJumpState(JumpState.Ground);
    }

    private void InitializeComponents()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
        _playerMove = GetComponent<PlayerMove>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _audioSource = GetComponent<AudioSource>();

        _jumpgagueAnimation = jumpgauge.transform.GetChild(0).transform.GetChild(1).GetComponent<Animation>();
        jumpgauge.SetActive(false);
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

    public void SetPlaySounds( bool playSounds)
    {
        _playSounds = playSounds;
    }


    //JumpEvent는 PlayerController에서 호출됨.
    public bool JumpEvent( JumpState state )
    {
        switch( state)
        {
            case JumpState.Ready:
                if(_canJump)
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
        ChangeJumpState(JumpState.Ground);

        _rigidbody.sharedMaterial = defaultPhyMat;
        _rigidbody.velocity = Vector2.zero;

        if (_playSounds)
        {
            _audioSource.clip = groundHitSound;
            _audioSource.Play();
        }
    }

    private void JumpReady()
    {
        ChangeJumpState(JumpState.Ready);
    }

    private void Jump()
    {
        jumpgauge.SetActive(false);
        ChangeJumpState(JumpState.Jump);

        _rigidbody.sharedMaterial = bouncePhyMat;

        if (_playSounds)
        {
            _audioSource.clip = jumpSound;
            _audioSource.Play();
        }

        _pressTime = Mathf.Clamp(_pressTime, 0f, _maxTime); // 최소 0초에서 최대 1초 동안 점프 기준을 정함
       
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

    public void ChangeJumpState( JumpState jumpState)
    {
        if (_currentState == jumpState) return;

        _currentState = jumpState;

        // Update 
        Sprite renderSprite = jumpReadySprite;

        bool player_move_enable = false;
        bool animator_enable = false;

        switch (_currentState)
        {
            case JumpState.Ready:
                renderSprite = jumpReadySprite;
                jumpgauge.SetActive(true);
                
                _jumpgagueAnimation.Play();
                break;

            case JumpState.Jump:
                renderSprite = jumpSprite;
                jumpgauge.SetActive(false);
                _canJump = false;
                break;

            case JumpState.Ground:
                player_move_enable = true;
                animator_enable = true;
                _canJump = true;
                break;
        }

        _playerMove.enabled = player_move_enable;
        _animator.enabled = animator_enable;
        _spriteRenderer.sprite = renderSprite;
    }

    public JumpState GetJumpState()
    {
        return _currentState;
    }

    public bool isJumped()
    {
        if (_currentState != JumpState.Ground) return true;
        return false;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (_currentState.Equals(JumpState.Ready))
            return;

        ChangeJumpState(JumpState.Falling);

        Vector2 contact_normal = other.GetContact(0).normal;
        //_rigidbody.velocity = Vector2.Reflect(-other.relativeVelocity, contact_normal) * reflectForce;

        if (contact_normal.y >= 0.8f)
        {
            Ground();
        }
        else
        {
            if (_playSounds)
            {
                _audioSource.clip = wallHitSound;
                _audioSource.Play();
            }
        }

    }
}