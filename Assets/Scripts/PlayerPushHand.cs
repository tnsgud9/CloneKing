using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPushHand : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    private PlayerJump _playerJump;
    private PlayerMove _playerMove;
    private Animator _animator;
    public GameObject pushHand;
    public bool skillReady = true;
    public float coolTime = 3f;
    public float pushForce;
    public Sprite playerPushSprite;
    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
        _playerJump = GetComponent<PlayerJump>();
        _playerMove = GetComponent<PlayerMove>();
    }

    // 이벤트는 Update문에 넣으면 된다.
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl) && skillReady)
        {
            _animator.enabled = false;
            _spriteRenderer.sprite = playerPushSprite;
            _playerMove.enabled = false;
            _playerJump.enabled = false;
            StartCoroutine(waitThenCallback(0.7f, () =>
            {
                _animator.enabled = true;
                _playerMove.enabled = true;
                _playerJump.enabled = true;
            }));
            
            
            Vector3 vec = transform.position;
            if (_spriteRenderer.flipX)
            {
                vec.x -= 0.1f;
                GameObject ph =Instantiate(pushHand,vec,Quaternion.Euler(0,180,0));
                ph.GetComponent<Rigidbody2D>().AddForce(Vector2.left * pushForce,ForceMode2D.Impulse);
                
            }
            else
            {
                vec.x += 0.1f;
                GameObject ph =Instantiate(pushHand,vec,Quaternion.Euler(0,0,0));
                ph.GetComponent<Rigidbody2D>().AddForce(Vector2.right * pushForce,ForceMode2D.Impulse);
            }
            skillReady = false;
        }
        
    }
    
    
    private IEnumerator waitThenCallback(float time, Action callback)
    {
        yield return new WaitForSeconds(time);
        callback();
    }

}
