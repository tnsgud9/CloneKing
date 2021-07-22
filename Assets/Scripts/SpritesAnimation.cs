using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpritesAnimation : MonoBehaviour
{
    
    public Sprite[] sprites;
    public float changeTime = 0.1f;

    private SpriteRenderer _spriteRenderer;
    private int spriteLength;
    private int spriteCount = 0;
    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        spriteLength = sprites.Length;
        InvokeRepeating("ChangeSprites",0,changeTime);
    }

    void ChangeSprites()
    {
        _spriteRenderer.sprite = sprites[spriteCount];
        spriteCount = (spriteCount + 1) % spriteLength;
    }
    
}