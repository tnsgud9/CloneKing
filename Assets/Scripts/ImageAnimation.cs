using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageAnimation : MonoBehaviour
{
    
    public Sprite[] sprites;
    public float changeTime = 0.1f;

    private Image _image;
    private int spriteLength;
    private int spriteCount = 0;
    private void Start()
    {
        _image = GetComponent<Image>();
        spriteLength = sprites.Length;
        InvokeRepeating("ChangeSprites",0,changeTime);
    }

    void ChangeSprites()
    {
        if (this.enabled)
        {
            _image.sprite = sprites[spriteCount];
            spriteCount = (spriteCount + 1) % spriteLength;
        }
    }

    public Sprite GetFirstSprite()
    {
        return sprites[0];
    }
}