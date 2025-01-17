using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

public class PlayerJump : MonoBehaviour
{
    
    
    // Components
    private PlayerMove _playerMove;
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;
    private Rigidbody2D _rigidbody;
    private AudioSource _audioSource;

    //Jump Variables
    private float _maxTime=1f;
    private float _pressTime;
    private JumpState _currentState;
    private bool _canJump = true;
    [SerializeField] private bool _playSounds = false; // If This is activated Player Sound is activated.

    //public float reflectForce = 0.5f;
    
    //Jump resources
    public Sprite jumpReadySprite;
    public Sprite jumpSprite;
    public Sprite fallSprite;
    public PhysicsMaterial2D defaultPhyMat;
    public PhysicsMaterial2D bouncePhyMat;
    public AudioClip jumpSound;
    public AudioClip wallHitSound;
    public AudioClip groundHitSound;
    public GameObject jumpGauge;
    private Animation _jumpGagueAnim;
    
    
    private void Start()
    {
        InitializeComponents();

        ChangeJumpState(JumpState.Ground);
    }
    private void Update()
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
    
    private void InitializeComponents()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
        _playerMove = GetComponent<PlayerMove>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _audioSource = GetComponent<AudioSource>();
        
        _jumpGagueAnim = jumpGauge.transform.GetChild(0).transform.GetChild(1).GetComponent<Animation>();
        jumpGauge.SetActive(false);
    }
    
    
    // SetPlaySounds is currently active and checked for control in the PhotonNetworks.
    // Need to delete Photon Networks Functions 
    public void SetPlaySounds( bool playSounds)
    {
        
        _playSounds = playSounds;
    }

    public bool DoubleJump()
    {
        if( _currentState == JumpState.Jump ||
            _currentState == JumpState.Falling)
        {
            _pressTime = _maxTime * 0.3f;
            Jump();

            return true;
        }

        return false;
    }

    //JumpEvent는 PlayerController에서 호출됨.
    public bool JumpEvent( JumpState state )
    {
        if (!enabled) return false;

        switch ( state)
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
        string[] layerNames = { "TileMap", "Player" };

        int layerIndex = 0;
        for( int i =0; i< layerNames.Length - 1; ++i)
        {
            layerIndex |= ( 1 << LayerMask.NameToLayer(layerNames[i]));
        }

        var hittedObjects = Physics2D.RaycastAll(transform.position, Vector2.down, 0.165f, layerIndex);
        for(int i =0; i < hittedObjects.Length; ++i)
        {
            var rayHit =  hittedObjects[i];

            if (rayHit.collider.gameObject == this.gameObject)
            {
                continue;
            }
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
        jumpGauge.SetActive(false);
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

        bool playerMoveEnable = false;
        bool animatorEnable = false;

        switch (_currentState)
        {
            case JumpState.Ready:
                renderSprite = jumpReadySprite;
                jumpGauge.SetActive(true);
                
                _jumpGagueAnim.Play();
                break;

            case JumpState.Jump:
                renderSprite = jumpSprite;
                jumpGauge.SetActive(false);
                _canJump = false;
                break;

            case JumpState.Falling:
                renderSprite = fallSprite;
                break;

            case JumpState.Ground:
                playerMoveEnable = true;
                animatorEnable = true;
                _canJump = true;
                break;
        }

        _playerMove.enabled = playerMoveEnable;
        _animator.enabled = animatorEnable;
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
        if (_currentState.Equals(JumpState.Ready) || _currentState.Equals(JumpState.Ground))
            return;

        ChangeJumpState(JumpState.Falling);

        Vector2 contactNormal = other.GetContact(0).normal;
        //_rigidbody.velocity = Vector2.Reflect(-other.relativeVelocity, contact_normal) * reflectForce;

        if (contactNormal.y >= 0.8f)
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