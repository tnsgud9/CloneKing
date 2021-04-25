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
    private float _jumpAngle;
    private float jTime;


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
        Debug.Log(Vec2ToAngle(new Vector2(1,1)));
        Debug.Log(Vec2ToAngle(new Vector2(-1,1)));
        Debug.Log( " angle :  "+ AngleToVec2(Vec2ToAngle(new Vector2(-1,1))));
        jTime = jumpMinForce;
        
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
            
            jTime += 5f*Time.deltaTime;
            _playerMove.enabled = false;
            _animator.enabled = false;
            _spriteRenderer.sprite = jumpReadySprite;
            //_jumpAngle = Mathf.LerpAngle()
            //Debug.Log(_jumpForce);
            
            //_jumpForce = Mathf.Lerp(jumpMinForce, jumpMaxForce, Time.deltaTime*5f);
            //_jumpForce = Mathf.Clamp(jTime, jumpMinForce, jumpMaxForce);
            
            
            //Debug.Log("Jump Angle : "+_jumpAngle);
            //Debug.Log("Jump Angle Vector 2 : "+RadianToVector2(_jumpAngle).normalized );
            //_jumpAngle = Mathf.LerpAngle(  45f, 75f, GetRatePer(jumpMinForce,jumpMaxForce,_jumpForce));

            
            _jumpForce = _jumpForce > jumpMaxForce ? jumpMaxForce : _jumpForce + (5f * Time.deltaTime);
            Debug.Log(_jumpForce);
            
            //Todo: Space 누른 시간의 따라 포물선 궤적의 각도가 변화하는 기능 구현.
            _jumpAngle = Mathf.Lerp(45f, 75f, GetRatePer(jumpMinForce, jumpMaxForce, _jumpForce));
            Debug.Log("Angle : "+_jumpAngle);
            Debug.Log("Angle2  : "+AngleToVec2(_jumpAngle).normalized);
            Debug.Log("Angle3  : "+AngleToVec2(_jumpAngle));




        }
        #endregion

        #region Jump
        if (Input.GetKeyUp(KeyCode.Space) && isGround && !isJump)
        {
            jTime = jumpMinForce;
            isJump = true;
            _spriteRenderer.sprite = jumpSprite;
            if (_spriteRenderer.flipX) // 왼쪽 보고 있을 때 
            {
                //_rigidbody.velocity = RadianToVector2(_jumpAngle).normalized * _jumpForce;
                //_rigidbody.velocity = AngleToVec2(_jumpAngle) * _jumpForce; 
                //오래 누를 수록 감소되어야함.
                _rigidbody.velocity = new Vector2(-1, 1) * _jumpForce;
            }
            else // 오른쪽 보고 있을떄
            {
                //_rigidbody.velocity = RadianToVector2(-_jumpAngle).normalized * _jumpForce;
                _rigidbody.velocity = new Vector2(1, 1) * _jumpForce;
            }

            _rigidbody.sharedMaterial = playerBouncePhysicsMaterial;
            _jumpForce = jumpMinForce;
        }
        #endregion
    }

    
    private void OnCollisionEnter2D(Collision2D other)
    {
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


