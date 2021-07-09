using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class CharaViewerItem : BaseSelectItem
{
    public ImageAnimation[] _charaAnimations;

    private Image _image;
    private Image _selectImage;
    private CharaType _charaType = CharaType.VirtualGuy;

    public override void Initialize(int enumIndex)
    {
        _charaType = (CharaType)enumIndex;

        InitializeComponents();
        UpdateWidgets();
    }


    public override void OnSelectedItem()
    {
        if (PhotonNetwork.connected)
        {
            PhotonNetwork.player.CustomProperties["CharaType"] = _charaType;
            UpdateWidgets();
        }
    }


    private void UpdateWidgets()
    {
        foreach ( var animation in _charaAnimations )
        {
            animation.enabled = false;
        }

        var targetAnimation = _charaAnimations[(int)_charaType];
        if ( _isSelection)
        {
            targetAnimation.enabled = true;
        }
        else
        {
            _image.sprite = targetAnimation.GetFirstSprite();
        }

        _selectImage.enabled = _isSelection;
    }

    private void Awake()
    {
        InitializeComponents();
    }

    void InitializeComponents()
    {
        _image = GetComponent<Image>();
        _selectImage = transform.GetChild(0).GetComponent<Image>();

        UpdateWidgets();
    }
}

