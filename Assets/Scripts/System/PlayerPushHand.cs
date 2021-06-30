using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerPushHand : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    private PlayerJump _playerJump;
    private PlayerMove _playerMove;
    private Animator _animator;
    public GameObject pushHand;
    public bool skillReady = true;
    public float pushForce;
    public Sprite playerPushSprite;
    
    
    public GameObject gauge;
    private Animation _gagueAnimation;

    
    private void Start()
    {
        InitializeComponents();
    }

    private void InitializeComponents()
    {
        //주의!! player가 갖고 있는 reload bar의 구성이 변경되면 오류가 발생할 수 있음.
        _gagueAnimation = gauge.transform.GetChild(0).transform.GetChild(1).GetComponent<Animation>();
        gauge.SetActive(false);
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
        _playerJump = GetComponent<PlayerJump>();
        _playerMove = GetComponent<PlayerMove>();
    }

    public void PushEvent()
    {
        if (skillReady)
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
            InstantiatePushHand();
            StartCoroutine(reload());
        }
    }

    private void InstantiatePushHand()
    {
        Vector3 position = transform.position;
        Vector2 direction = Vector2.left;
        Quaternion quaternion = Quaternion.Euler(0, 0, 0);

        if (_spriteRenderer.flipX)
        {
            position.x -= 0.065f;
            quaternion = Quaternion.Euler(0, 180, 0);
        }
        else
        {
            position.x += 0.065f;
            direction = Vector2.right;
        }

        GameObject go = Instantiate(pushHand, position, quaternion);

        var collider        = GetComponent<Collider2D>();
        var go_collider     = go.GetComponent<Collider2D>();

        Physics2D.IgnoreCollision(collider, go_collider,true);
        go.GetComponent<Rigidbody2D>().velocity = direction * pushForce;

        skillReady = false;
    }
    
    private IEnumerator reload()
    {
        gauge.SetActive(true);
        _gagueAnimation.Play();
        yield return new WaitForSeconds(10f);
        skillReady = true;
        gauge.SetActive(false);
    }

    private IEnumerator waitThenCallback(float time, Action callback)
    {
        yield return new WaitForSeconds(time);
        callback();
    }

}
