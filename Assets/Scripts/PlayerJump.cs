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
    private float _jumpForce;
    private float _jumpAngle;
    private bool groundCallback = true;


    public Sprite jumpReadySprite;
    public Sprite jumpSprite;
    public float jumpMaxForce = 10f;
    public float jumpMinForce = 3f;
    
    
    public PhysicsMaterial2D defaultPhyMat;
    public PhysicsMaterial2D bouncePhyMat;

    public enum jumpState
    {
        Ready,
        Jump,
        Ground
    };

    [SerializeField] public jumpState state;
    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
        _playerMove = GetComponent<PlayerMove>();
        _rigidbody = GetComponent<Rigidbody2D>();
        state = jumpState.Ground;
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Space)) JumpReady();
        if (Input.GetKeyUp(KeyCode.Space)) Jump();
    }
    private void Ground()
    {
        state = jumpState.Ground;
        Debug.Log("STATE : " + state);
        _rigidbody.sharedMaterial = defaultPhyMat;
        _playerMove.enabled = true;
        _animator.enabled = true;
        _rigidbody.velocity = Vector2.zero;
        
    }
    private void JumpReady()
    {
        state = jumpState.Ready;
        Debug.Log("STATE : "+state);
        
        _playerMove.enabled = false;
        _animator.enabled = false;
        _spriteRenderer.sprite = jumpReadySprite;
        
        //버튼 누를때 각도 힘 조절 이벤트
        _jumpForce = _jumpForce > jumpMaxForce ? jumpMaxForce : _jumpForce + (5f * Time.deltaTime);
        
    }

    private void Jump()
    {
        state = jumpState.Jump;
        Debug.Log("STATE : "+state);
        _rigidbody.sharedMaterial = bouncePhyMat;
        _playerMove.enabled = false;
        _animator.enabled = false;
        _spriteRenderer.sprite = jumpSprite;
        
        // 점프 이벤트
        if (_spriteRenderer.flipX) // 왼쪽 보고 있을 때 
        {
            _rigidbody.velocity = new Vector2(-1, 1) * _jumpForce;
        }
        else // 오른쪽 보고 있을떄
        {
            
            _rigidbody.velocity = new Vector2(1, 1) * _jumpForce;
        }
        _jumpForce = jumpMinForce;
        
        
        // callback Event 
        groundCallback = false;
        StartCoroutine(waitThenCallback(0.1f, () =>
        {
            groundCallback = true;
        }));
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        foreach (ContactPoint2D hitPos in other.contacts)
        {
            if (hitPos.normal.y >= 1f && groundCallback) Ground();
        }
    }
    private void OnCollisionStay2D(Collision2D other)
    {
        foreach (ContactPoint2D hitPos in other.contacts)
        {
            if (hitPos.normal.y >= 1f && groundCallback) Ground();
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


