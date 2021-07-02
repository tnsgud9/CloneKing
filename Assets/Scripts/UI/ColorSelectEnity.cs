using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;


public class ColorSelectEnity : MonoBehaviour
{
    private Image _image;
    private Image _selectImage;
    private Button _button; 
    private PlayerColor _color;
    private bool _isSelection = false;

    private Action<ColorSelectEnity> clickAction;

    public void SetupColor(ColorSelector selector, PlayerColor color)
    {
        _color = color;
        _button.onClick.RemoveAllListeners();
        _button.onClick.AddListener(OnClickButton);

        clickAction = selector.OnSelectedColor;

        UpdateWidgets();
    }

    public void OnClickButton()
    {
        clickAction.Invoke(this);
    }

    public void SetSelection( bool isSelection)
    {
        if (PhotonNetwork.connected)
        {
            PhotonNetwork.player.CustomProperties["Color"] = _color;

            _isSelection = isSelection;

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
        _button = GetComponent<Button>();
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
