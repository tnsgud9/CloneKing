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
    public bool _groundCallback = true;


    public Sprite jumpReadySprite;
    public Sprite jumpSprite;
    
    
    public PhysicsMaterial2D defaultPhyMat;
    public PhysicsMaterial2D bouncePhyMat;

    public AudioClip jumpSound;
    public AudioClip wallHitSound;
    public AudioClip groundHitSound;

    public enum jumpState
    {
        Ready,
        Jump,
        Ground,
        Goal
    };

    [SerializeField] public jumpState state;
    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
        _playerMove = GetComponent<PlayerMove>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _audioSource = GetComponent<AudioSource>();
        state = jumpState.Ground;
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Space)
            && state != jumpState.Jump) JumpReady();
        if (Input.GetKeyUp(KeyCode.Space) 
            && state == jumpState.Ready) Jump();
        Debug.DrawRay(transform.position,Vector3.down*0.165f,Color.blue);
        if (Physics2D.Raycast(transform.position, Vector2.down, 0.165f, 1 << LayerMask.NameToLayer("TileMap")) 
            && _groundCallback)
        {
            
            Debug.DrawRay(transform.position,Vector3.down*0.165f,Color.red);
            Ground();
        }
        
    }
    private void Ground()
    {
        Debug.Log("Ground!!!!!!");
        state = jumpState.Ground;
        //Debug.Log("STATE : " + state);
        _rigidbody.sharedMaterial = defaultPhyMat;
        _playerMove.enabled = true;
        _animator.enabled = true;
        _rigidbody.velocity = Vector2.zero;
        
    }
    private void JumpReady()
    {
        state = jumpState.Ready;
        //Debug.Log("STATE : "+state);
        
        _playerMove.enabled = false;
        _animator.enabled = false;
        _groundCallback = false;

        _spriteRenderer.sprite = jumpReadySprite;
        
        //버튼 누를때 각도 힘 조절 이벤트
        _pressTime += Time.deltaTime;
        //Debug.Log("JUMP READY : "+ _pressTime);
        //_jumpForce = _jumpForce > jumpMaxForce ? jumpMaxForce : _jumpForce + (5f * Time.deltaTime);

    }

    private void Jump()
    {
        state = jumpState.Jump;
        Debug.Log("STATE : "+state);
        _rigidbody.sharedMaterial = bouncePhyMat;
        _playerMove.enabled = false;
        _animator.enabled = false;
        _spriteRenderer.sprite = jumpSprite;

        _audioSource.clip = jumpSound;
        _audioSource.Play();
        
        
        _pressTime = Mathf.Clamp(_pressTime, 0f, 1f); // 최소 0초에서 최대 1초 동안 점프 기준을 정함
        //Debug.Log("Press Time : " + _pressTime);
        float y = Mathf.Lerp(3f, 7f, _pressTime);
        float x = Mathf.Lerp(1f, 4f, _pressTime);
        //float y = Mathf.Clamp(_yJumpForce, 2f, 7f);
        //float y = GetRatePer(jumpMinForce, jumpMaxForce, _jumpForce);
        //Debug.Log(" JUMP : "+ y);
        // 점프 이벤트
        if (_spriteRenderer.flipX) // 왼쪽 보고 있을 때 
        {
            //Debug.Log("Velocity Vector : "+ new Vector2(-x,y));
            _rigidbody.velocity = new Vector2(-x, y);
            //_rigidbody.velocity = new Vector2(-1, 1) * _jumpForce;
        }
        else // 오른쪽 보고 있을떄
        {
            
            //Debug.Log("Velocity Vector : "+ new Vector2(x,y));
            _rigidbody.velocity = new Vector2(x, y);
            //_rigidbody.velocity = new Vector2(1, 1) * _jumpForce;
        }

        _pressTime = 0f;
        
        
        // callback Event 
        _groundCallback = false;
        StartCoroutine(waitThenCallback(0.1f, () =>
        {
            _groundCallback = true;
        }));
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        Vector2 direction = other.GetContact(0).normal; 
        if (direction.y >= 0.8f && _groundCallback)
        {
            _audioSource.clip = groundHitSound;
            _audioSource.Play();
            Debug.Log("enter col!");
            Ground();
        }
        else
        {
            _audioSource.clip = wallHitSound;
            _audioSource.Play();
            
        }
        /*
        foreach (ContactPoint2D hitPos in other.contacts)
        {
            Vector2 direction = hitPos.normal;
            if (direction.x == 1) print("“right”");
            if (direction.x == -1) print("“left”");
            if (direction.y == 1) print("“up”");
            if (direction.y == -1) print("“down”");
        }
        */
        
        
    }
    private void OnCollisionStay2D(Collision2D other)
    {
        Vector2 direction = other.GetContact(0).normal; 
        Debug.Log(direction);
        if (direction.y >= 0.8f && _groundCallback)
        {
            Debug.Log("stay col!");
            Ground();
        }
    }

    
    private IEnumerator waitThenCallback(float time, Action callback)
    {
        yield return new WaitForSeconds(time);
        callback();
    }
    //=================================================================================================================
    public static float Vec2ToAngle(Vector2 dir)
    {
        return Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
    }

    public static Vector2 AngleToVec2(float angle)
    {
        return new Vector2(Mathf.Sin(angle), Mathf.Sin(angle));
    }
    public static Vector2 RadianToVector2(float radian)
    {
        return new Vector2(Mathf.Cos(radian), Mathf.Sin(radian));
    }

    public static float GetRatePer(float min, float max, float value)
    {
        return (value - min) / (max-min); 
    }
    
    
    
}


