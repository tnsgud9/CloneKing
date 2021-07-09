using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;


public class ColorSelectItem : BaseSelectItem
{
    private Image _image;
    private Image _selectImage;
    private PlayerColor _color;

    public override void Initialize(int enumIndex)
    {
        _color = (PlayerColor)enumIndex;

        UpdateWidgets();
    }

    public override void OnUpdateSelection()
    {
        if (PhotonNetwork.connected)
        {
            if (_isSelection)
            {
                PhotonNetwork.player.CustomProperties["Color"] = _color;
                PhotonNetwork.player.SetCustomProperties(PhotonNetwork.player.CustomProperties);
            }
            UpdateWidgets();
        }
    }


    private void UpdateWidgets()
    {
        _image.color = _color.PlayerColorToColor();

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
    }
}
