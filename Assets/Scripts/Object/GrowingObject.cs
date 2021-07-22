using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowingObject : SynchronizedObject
{
    public float startCountY = 0.0f;
    public float destCountY = 250.0f;
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
    protected override void Update()
    {
        base.Update();

        _elapsedTime += Time.deltaTime;

        if (_elapsedTime > 0.0f)
        {
            float factor = Mathf.Clamp01((_elapsedTime )/ growTime);
            _spriteRenderer.size = new Vector2(_spriteRenderer.size.x, Mathf.Lerp(startCountY, destCountY, EasingFunction.EaseInQuart(0.0f, 1.0f, factor)));
        }
    }
}
