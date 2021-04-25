using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushHand : MonoBehaviour
{
    public float destroyTime = 2f;
    public float hitDestroyTime = 0.3f;
    private Rigidbody2D _rigidbody2D;
    private IEnumerator waitThenCallback(float time, Action callback)
    {
        yield return new WaitForSeconds(time);
        callback();
    }

    private void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        StartCoroutine(waitThenCallback(destroyTime, () =>
        {
            Destroy(this.gameObject);
        }));
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        
        if (other.gameObject.CompareTag("Player"))
        {
            //_rigidbody2D.velocity = Vector2.zero; // 맞으면 이동이 멈춤
            StopAllCoroutines();
            StartCoroutine(waitThenCallback(hitDestroyTime, () =>
            {
                Destroy(this.gameObject);
            }));
        }
    }
}
