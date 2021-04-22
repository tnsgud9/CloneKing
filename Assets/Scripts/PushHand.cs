using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushHand : MonoBehaviour
{
    private Rigidbody2D _rigidbody2D;
    private IEnumerator waitThenCallback(float time, Action callback)
    {
        yield return new WaitForSeconds(time);
        callback();
    }

    private void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        StartCoroutine(waitThenCallback(3f, () =>
        {
            Destroy(this.gameObject);
        }));
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        
        Debug.Log(other.gameObject.name);
        if (other.gameObject.layer == LayerMask.NameToLayer("TileMap"))
        {
            _rigidbody2D.velocity = Vector2.zero;
            StopAllCoroutines();
            StartCoroutine(waitThenCallback(0.3f, () =>
            {
                Destroy(this.gameObject);
            }));
        }
        if (other.gameObject.CompareTag("Player"))
        {
            _rigidbody2D.velocity = Vector2.zero;
            StopAllCoroutines();
            StartCoroutine(waitThenCallback(0.3f, () =>
            {
                Destroy(this.gameObject);
            }));
        }

        
    }
}
