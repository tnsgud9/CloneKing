using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowingObject : MonoBehaviour
{
    public float startCountY = 0.0f;
    public float destCountY = 100.0f;
    public float growTime = 5.0f;

    private float _elapsedTime = 0.0f;
    private SpriteRenderer _spriteRenderer = null;
    
    void Start()
    {
        InitializeComponents();
    }

    private void InitializeComponents()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        _elapsedTime += Time.deltaTime;

        if (_elapsedTime > 1.0f)
        {
            float factor = Mathf.Clamp01((_elapsedTime - 1.0f )/ growTime);
            _spriteRenderer.size = new Vector2(1.0f, Mathf.Lerp(startCountY, destCountY, EasingFunction.Linear(0.0f, 1.0f, factor)));
        }
    }
}
